namespace Atlas.Dna.Utilities;

using Atlas.Dna.Helpers;
using MessagePack;
using System.Reflection;

public static class CityDataLoader
{
    public static CityDataBundle? Data { get; private set; }

    private static readonly object Locker = new();

    public static void Load()
    {
        if (IsLoaded) return;

        lock (Locker)
        {
            if (IsLoaded) return;

            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith("worldcities.bin")) ?? throw new Exception("worldcities.bin not found in embedded resources.");

            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            var options = MessagePackSerializerOptions.Standard
                .WithCompression(MessagePackCompression.Lz4Block);

            Data = MessagePackSerializer.Deserialize<CityDataBundle>(stream, options);

            var ids = Data.Cities.Select((_, i) => i).Cast<object>();
            var lats = Data.Cities.Select(city => (double)city.Lat);
            var lons = Data.Cities.Select(city => (double)city.Lng);

            GeoIndexManager.CreateIndex("!citydata", ids, lats, lons);
        }
    }

    public static bool IsLoaded => Data != null;

    public static CityCompact? GetNearestCity(float lat, float lng)
    {
        if (Data == null || Data.Cities == null) return null;

        var indx = GeoIndexManager.FindNearest("!citydata", lat, lng, 1);

        if (indx == null || indx.Count == 0) return null;

        var nearest = Data.Cities[(int)indx[0].Id];

        return nearest;
    }
}

[MessagePackObject]
public class AdminCompact
{
    [Key(0)] public int AdminId = default!;
    [Key(1)] public string AdminName = default!;
    [Key(2)] public string AdminCode = default!;
    [Key(3)] public string AdminType = default!;
}

[MessagePackObject]
public class CityCompact
{
    [Key(0)] public float Lat;
    [Key(1)] public float Lng;
    [Key(2)] public int City;
    [Key(3)] public byte Country;
    [Key(4)] public int Timezone;
    [Key(5)] public int Admin;
}

[MessagePackObject]
public class CityDataBundle
{
    [Key(0)] public List<CityCompact> Cities = new();
    [Key(1)] public List<CountryCompact> Countries = new();
    [Key(2)] public List<AdminCompact> Admins = new();
    [Key(3)] public List<string> CityStringPool = new();
    [Key(4)] public List<string> TimeZoneStringPool = new();
}

[MessagePackObject]
public class CountryCompact
{
    [Key(0)] public byte CountryId = default!;
    [Key(1)] public string CountryName = default!;
    [Key(2)] public string CountryCode = default!;
}