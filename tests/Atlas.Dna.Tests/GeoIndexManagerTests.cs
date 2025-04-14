using Atlas.Dna.Helpers;

namespace Atlas.Dna.Tests;

public class GeoIndexManagerTests
{
    [Fact]
    public void GeoIndexManager_CanCreateAndFindNearest()
    {
        GeoIndexManager.CreateIndex("Test", new[] { "A", "B" }, new[] { 0.0, 10.0 }, new[] { 0.0, 10.0 });

        var results = GeoIndexManager.FindNearest("Test", 0.1, 0.1, 1);
        Assert.Single(results);
        Assert.Equal("A", results[0].Id);

        GeoIndexManager.ClearIndex("Test");
    }
}