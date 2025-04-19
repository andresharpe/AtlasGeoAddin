using CsvHelper;
using CsvHelper.Configuration;
using MessagePack;
using System.Globalization;

namespace Atlas.BuildBin;

[MessagePackObject]
public class CityCompact
{
    [Key(0)] public float Lat;
    [Key(1)] public float Lng;
    [Key(2)] public int City;
    [Key(3)] public int Country;
    [Key(4)] public int Timezone;
    [Key(5)] public int AdminType;
}

[MessagePackObject]
public class CityDataBundle
{
    [Key(0)] public List<CityCompact> Cities = [];
    [Key(1)] public List<string> StringPool = [];
}

internal class Program
{
    private static void Main()
    {
        var inputPath = "./data/worldcities.csv";
        var outputPath = "./data/worldcities.bin";

        var bundle = new CityDataBundle();
        var stringIndex = new Dictionary<string, int>();

        using var reader = new StreamReader(inputPath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            BadDataFound = null,
            MissingFieldFound = null,
            IgnoreBlankLines = true
        });

        var records = csv.GetRecords<dynamic>();

        foreach (var record in records)
        {
            try
            {
                if (!int.TryParse(record.population, NumberStyles.Any, CultureInfo.InvariantCulture, out int population) || population < 5000)
                    continue;

                float lat = float.Parse(record.lat, CultureInfo.InvariantCulture);
                float lng = float.Parse(record.lng, CultureInfo.InvariantCulture);

                int cityIdx = GetOrAdd(stringIndex, record.city_ascii, bundle.StringPool);
                int countryIdx = GetOrAdd(stringIndex, record.country, bundle.StringPool);
                int timezoneIdx = GetOrAdd(stringIndex, record.timezone, bundle.StringPool);
                int adminTypeIdx = GetOrAdd(stringIndex, record.admin_type, bundle.StringPool);

                bundle.Cities.Add(new CityCompact
                {
                    Lat = lat,
                    Lng = lng,
                    City = cityIdx,
                    Country = countryIdx,
                    Timezone = timezoneIdx,
                    AdminType = adminTypeIdx
                });
            }
            catch
            {
                // Skip bad rows silently
            }
        }

        var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
        var bytes = MessagePackSerializer.Serialize(bundle, options);
        File.WriteAllBytes(outputPath, bytes);

        Console.WriteLine($"✅ Saved compressed city data to: {outputPath}");
        Console.WriteLine($"   → Total cities: {bundle.Cities.Count}");
        Console.WriteLine($"   → Unique strings in pool: {bundle.StringPool.Count}");
    }

    private static int GetOrAdd(Dictionary<string, int> map, string value, List<string> pool)
    {
        if (!map.TryGetValue(value, out int index))
        {
            index = pool.Count;
            map[value] = index;
            pool.Add(value);
        }
        return index;
    }
}