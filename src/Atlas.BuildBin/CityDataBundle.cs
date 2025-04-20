using MessagePack;

namespace Atlas.BuildBin;

[MessagePackObject]
public class CityDataBundle
{
    [Key(0)] public List<CityCompact> Cities = [];
    [Key(1)] public List<CountryCompact> Countries = [];
    [Key(2)] public List<AdminCompact> Admins = [];
    [Key(3)] public List<string> CityStringPool = [];
    [Key(4)] public List<string> TimeZoneStringPool = [];
}