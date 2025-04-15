using Atlas.Dna.Helpers;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class LookupFunctions
{
    [ExcelFunction(Name = "GEO_LOOKUPNEARESTK", Description = "Finds the nearest K IDs in a range of lat/lon entries for a source lat/lon.")]
    public static object GeoLookupNearestK(
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to lookup into.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to lookup into.")] object[,] lon_range,
        [ExcelArgument(Name = "id_range", Description = "A 2D array of IDs corresponding to the latitudes and longitudes to lookup into.")] object[,] id_range,
        [ExcelArgument(Name = "[k]", Description = "The number of nearest entries to return.")] int k,
        [ExcelArgument(Name = "[return_distance]", Description = "Optional boolean parameter to return the distance in km along with the ID.")] bool return_distance = false)
    {
        try
        {
            int latsCount = lat_range.GetLength(0);
            int lonsCount = lon_range.GetLength(0);
            int idsCount = id_range.GetLength(0);

            if (latsCount != lonsCount || latsCount != idsCount)
            {
                return ExcelError.ExcelErrorValue;
            }

            var distances = new List<(double Distance, object Id)>();

            for (int i = 0; i < latsCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    double dist = GeoCalculations.HaversineDistance(lat, lon, validLat, validLon);
                    distances.Add((dist, id_range[i, 0]));
                }
            }

            var nearestK = distances.OrderBy(d => d.Distance).Take(k).ToList();

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

    [ExcelFunction(Name = "GEO_LOOKUPFURTHESTK", Description = "Finds the furthest K IDs in a range of lat/lon entries for a source lat/lon.")]
    public static object GeoLookupFurthestK(
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to lookup into.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to lookup into.")] object[,] lon_range,
        [ExcelArgument(Name = "id_range", Description = "A 2D array of IDs corresponding to the latitudes and longitudes to lookup into.")] object[,] id_range,
        [ExcelArgument(Name = "[k]", Description = "The number of furthest entries to return.")] int k,
        [ExcelArgument(Name = "[return_distance]", Description = "Optional boolean parameter to return the distance in km along with the ID.")] bool return_distance = false)
    {
        try
        {
            int latsCount = lat_range.GetLength(0);
            int lonsCount = lon_range.GetLength(0);
            int idsCount = id_range.GetLength(0);

            if (latsCount != lonsCount || latsCount != idsCount)
            {
                return ExcelError.ExcelErrorValue;
            }

            var distances = new List<(double Distance, object Id)>();

            for (int i = 0; i < latsCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    double dist = GeoCalculations.HaversineDistance(lat, lon, validLat, validLon);
                    distances.Add((dist, id_range[i, 0]));
                }
            }

            var furthestK = distances.OrderByDescending(d => d.Distance).Take(k).ToList();

            if (return_distance)
            {
                var result = new object[k, 2];
                for (int i = 0; i < k; i++)
                {
                    result[i, 0] = furthestK[i].Id;
                    result[i, 1] = furthestK[i].Distance;
                }
                return result;
            }
            else
            {
                var result = new object[k];
                for (int i = 0; i < k; i++)
                {
                    result[i] = furthestK[i].Id;
                }
                return result;
            }
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_LOOKUPNEAREST", Description = "Finds the nearest ID in a range of lat/lon entries for a source lat/lon.")]
    public static object GeoLookupNearest(
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to lookup into.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to lookup into.")] object[,] lon_range,
        [ExcelArgument(Name = "id_range", Description = "A 2D array of IDs corresponding to the latitudes and longitudes to lookup into.")] object[,] id_range,
        [ExcelArgument(Name = "[return_distance]", Description = "Optional boolean parameter to return the distance in km along with the ID.")] bool return_distance = false)
    {
        try
        {
            int latsCount = lat_range.GetLength(0);
            int lonsCount = lon_range.GetLength(0);
            int idsCount = id_range.GetLength(0);

            if (latsCount != lonsCount || latsCount != idsCount)
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                if (return_distance)
                {
                    return new object[] { ExcelError.ExcelErrorValue, ExcelError.ExcelErrorValue };
                }
                return ExcelError.ExcelErrorValue;
            }

            double nearestDistance = double.MaxValue;
            object nearestId = ExcelError.ExcelErrorNA;

            for (int i = 0; i < latsCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    double dist = GeoCalculations.HaversineDistance(lat, lon, validLat, validLon);
                    if (dist < nearestDistance)
                    {
                        nearestId = id_range[i, 0];
                        nearestDistance = dist;
                    }
                }
            }

            if (return_distance)
            {
                return new object[] { nearestId, nearestDistance };
            }
            else
            {
                return nearestId;
            }
        }
        catch
        {
            if (return_distance)
            {
                return new object[] { ExcelError.ExcelErrorValue, ExcelError.ExcelErrorValue };
            }
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_LOOKUPFURTHEST", Description = "Finds the furthest ID in a range of lat/lon entries for a source lat/lon.")]
    public static object GeoLookupFurthest(
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to lookup into.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to lookup into.")] object[,] lon_range,
        [ExcelArgument(Name = "id_range", Description = "A 2D array of IDs corresponding to the latitudes and longitudes to lookup into.")] object[,] id_range,
        [ExcelArgument(Name = "[return_distance]", Description = "Optional boolean parameter to return the distance in km along with the ID.")] bool return_distance = false)
    {
        try
        {
            int latsCount = lat_range.GetLength(0);
            int lonsCount = lon_range.GetLength(0);
            int idsCount = id_range.GetLength(0);

            if (latsCount != lonsCount || latsCount != idsCount)
            {
                return ExcelError.ExcelErrorValue;
            }

            double furthestDistance = double.MinValue;
            object furthestId = ExcelError.ExcelErrorNA;

            for (int i = 0; i < latsCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    double dist = GeoCalculations.HaversineDistance(lat, lon, validLat, validLon);
                    if (dist > furthestDistance)
                    {
                        furthestId = id_range[i, 0];
                        furthestDistance = dist;
                    }
                }
            }

            if (return_distance)
            {
                return new object[] { furthestId, furthestDistance };
            }
            else
            {
                return furthestId;
            }
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_LOOKUPWITHINRADIUS", Description = "Finds the IDs within a specified radius in a range of lat/lon entries for a source lat/lon.")]
    public static object GeoLookupWithinRadius(
        [ExcelArgument(Name = "lat", Description = "The source latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The source longitude value to lookup.")] double lon,
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to lookup into.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to lookup into.")] object[,] lon_range,
        [ExcelArgument(Name = "id_range", Description = "A 2D array of IDs corresponding to the latitudes and longitudes to lookup into.")] object[,] id_range,
        [ExcelArgument(Name = "radius", Description = "The radius within which to find the IDs.")] double radius,
        [ExcelArgument(Name = "[unit]", Description = "The unit of the radius (\"km\" or \"mi\").")] string unit = "km")
    {
        try
        {
            int latsCount = lat_range.GetLength(0);
            int lonsCount = lon_range.GetLength(0);
            int idsCount = id_range.GetLength(0);

            if (latsCount != lonsCount || latsCount != idsCount)
            {
                return ExcelError.ExcelErrorValue;
            }

            var result = new List<object>();

            for (int i = 0; i < latsCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    double dist = GeoCalculations.HaversineDistance(lat, lon, validLat, validLon, unit);
                    if (dist <= radius)
                    {
                        result.Add(id_range[i, 0]);
                    }
                }
            }

            return result.ToArray();
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }
}