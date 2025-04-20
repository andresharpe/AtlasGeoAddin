using MessagePack;

namespace Atlas.BuildBin;

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