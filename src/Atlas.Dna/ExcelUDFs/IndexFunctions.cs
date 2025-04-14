using Atlas.Dna.Helpers;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class IndexFunctions
{
    [ExcelFunction(Name = "GEO_INDEXCREATE", Description = "Creates an optimized spatial index for fast lookups.")]
    public static object GeoIndexCreate(
        [ExcelArgument(Name = "index_name", Description = "The name of the index to create.")] string indexName,
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to index.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to index.")] object[,] lon_range,
        [ExcelArgument(Name = "id_range", Description = "A 2D array of IDs corresponding to the latitudes and longitudes to index.")] object[,] id_range)
    {
        try
        {
            int rowCount = id_range.GetLength(0);
            if (rowCount != lat_range.GetLength(0) || rowCount != lon_range.GetLength(0))
            {
                return ExcelError.ExcelErrorValue;
            }

            var ids = new List<string>();
            var lats = new List<double>();
            var lons = new List<double>();

            for (int i = 0; i < rowCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    ids.Add(id_range[i, 0] as string ?? id_range[i, 0].ToString() ?? "#N/A");
                    lats.Add(validLat);
                    lons.Add(validLon);
                }
            }

            GeoIndexManager.CreateIndex(indexName, ids, lats, lons);
            return $"KD-tree '{indexName}' built with {rowCount} points.";
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_INDEXCLEAR", Description = "Removes the specified spatial index from memory.")]
    public static object GeoIndexClear(
        [ExcelArgument(Name = "index_name", Description = "The name of the index to clear.")] string indexName)
    {
        try
        {
            GeoIndexManager.ClearIndex(indexName);
            return $"KD-tree '{indexName}' cleared.";
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_INDEXLOOKUPNEAREST", Description = "Finds the nearest ID in an indexed range for a source lat/lon.")]
    public static object GeoIndexLookupNearest(
        [ExcelArgument(Name = "index_name", Description = "The name of the index to search.")] string indexName,
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "[return_distance]", Description = "Optional boolean parameter to return the distance in km along with the ID.")] bool return_distance = false)
    {
        try
        {
            var nearest = GeoIndexManager.FindNearest(indexName, lat, lon, 1).FirstOrDefault();
            if (nearest == default)
            {
                return ExcelError.ExcelErrorNA;
            }

            if (return_distance)
            {
                return new object[] { nearest.Id, nearest.Distance };
            }
            else
            {
                return nearest.Id;
            }
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_INDEXLOOKUPNEARESTK", Description = "Finds the nearest K IDs in an indexed range for a source lat/lon.")]
    public static object GeoIndexLookupNearestK(
        [ExcelArgument(Name = "index_name", Description = "The name of the index to search.")] string indexName,
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "[k]", Description = "The number of nearest entries to return.")] int k,
        [ExcelArgument(Name = "[return_distance]", Description = "Optional boolean parameter to return the distance in km along with the ID.")] bool return_distance = false)
    {
        try
        {
            var nearestK = GeoIndexManager.FindNearest(indexName, lat, lon, k);
            if (nearestK == null || nearestK.Count == 0)
            {
                return ExcelError.ExcelErrorNA;
            }

            if (return_distance)
            {
                var result = new object[k, 2];
                for (int i = 0; i < k; i++)
                {
                    result[i, 0] = nearestK[i].Id;
                    result[i, 1] = nearestK[i].Distance;
                }
                return result;
            }
            else
            {
                var result = new object[k];
                for (int i = 0; i < k; i++)
                {
                    result[i] = nearestK[i].Id;
                }
                return result;
            }
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }
}