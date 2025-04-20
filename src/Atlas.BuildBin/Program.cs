using CsvHelper;
using CsvHelper.Configuration;
using MessagePack;
using System.Globalization;
using System.Numerics;

namespace Atlas.BuildBin;

internal class Program
{
    private static void Main()
    {
        var inputPath = "./data/worldcities.csv";
        var outputPath = "./data/worldcities.bin";

        var bundle = new CityDataBundle();

        var cityStringPoolIndex = new Dictionary<string, int>();
        var timeZoneStringPoolIndex = new Dictionary<string, int>();
        var adminPoolIndex = new Dictionary<string, AdminCompact>();
        var countryPoolIndex = new Dictionary<string, CountryCompact>();

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

                var cityIdx = GetOrAdd(cityStringPoolIndex, record.city_ascii, bundle.CityStringPool);
                var timezoneIdx = GetOrAdd(timeZoneStringPoolIndex, record.timezone, bundle.TimeZoneStringPool);
                var adminIdx = GetOrAdd(adminPoolIndex, record.admin_code, record.admin_name_ascii, record.admin_type, bundle.Admins);
                var countryIdx = GetOrAdd(countryPoolIndex, record.iso2, record.country, bundle.Countries);

                bundle.Cities.Add(new CityCompact
                {
                    Lat = lat,
                    Lng = lng,
                    City = cityIdx,
                    Country = countryIdx,
                    Admin = adminIdx,
                    Timezone = timezoneIdx,
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
        Console.WriteLine($"   → Unique city strings in pool: {bundle.CityStringPool.Count}");
        Console.WriteLine($"   → Unique countries in pool: {bundle.Countries.Count}");
        Console.WriteLine($"   → Unique time zone strings in pool: {bundle.TimeZoneStringPool.Count}");
        Console.WriteLine($"   → Unique admins in pool: {bundle.Admins.Count}");
    }

    private static T GetOrAdd<T>(Dictionary<string, T> map, string value, List<string> pool) where T : INumber<T>
    {
        if (!map.TryGetValue(value, out T? index))
        {
            index = T.CreateChecked(pool.Count);
            map[value] = index;
            pool.Add(value);
        }
        return index;
    }

    private static byte GetOrAdd(Dictionary<string, CountryCompact> map, string countryIso2, string countryName, List<CountryCompact> pool)
    {
        if (!map.TryGetValue(countryIso2, out CountryCompact? country))
        {
            country = new CountryCompact() { CountryId = (byte)map.Count, CountryName = countryName, CountryCode = countryIso2 };
            map[countryIso2] = country;
            pool.Add(country);
        }
        return country.CountryId;
    }

    private static int GetOrAdd(Dictionary<string, AdminCompact> map, string adminCode, string adminName, string adminType, List<AdminCompact> pool)
    {
        if (!map.TryGetValue(adminCode, out AdminCompact? admin))
        {
            admin = new AdminCompact() { AdminId = map.Count, AdminName = adminName, AdminCode = adminCode, AdminType = adminType };
            map[adminCode] = admin;
            pool.Add(admin);
        }
        return admin.AdminId;
    }
}