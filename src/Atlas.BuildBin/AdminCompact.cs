using MessagePack;

namespace Atlas.BuildBin;

[MessagePackObject]
public class AdminCompact
{
    [Key(0)] public int AdminId = default!;
    [Key(1)] public string AdminName = default!;
    [Key(2)] public string AdminCode = default!;
    [Key(3)] public string AdminType = default!;
}