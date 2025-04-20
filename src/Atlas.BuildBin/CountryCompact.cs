using MessagePack;

namespace Atlas.BuildBin;

[MessagePackObject]
public class CountryCompact
{
    [Key(0)] public byte CountryId = default!;
    [Key(1)] public string CountryName = default!;
    [Key(2)] public string CountryCode = default!;
}