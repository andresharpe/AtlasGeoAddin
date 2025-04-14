using Atlas.Dna.Helpers;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class DistanceFunctions
{
    [ExcelFunction(Name = "GEO_DISTANCE", Description = "Calculates the distance between two geographic coordinates.")]
    public static object GeoDistance(
        [ExcelArgument(Name = "lat1", Description = "The latitude of the first coordinate.")] double lat1,
        [ExcelArgument(Name = "lon1", Description = "The longitude of the first coordinate.")] double lon1,
        [ExcelArgument(Name = "lat2", Description = "The latitude of the second coordinate.")] double lat2,
        [ExcelArgument(Name = "lon2", Description = "The longitude of the second coordinate.")] double lon2,
        [ExcelArgument(Name = "[unit]", Description = "The unit of distance: 'km' (default) or 'mi'.")] string unit = "km")
    {
        try
        {
            return GeoCalculations.HaversineDistance(lat1, lon1, lat2, lon2, unit);
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_TOTALDISTANCE", Description = "Calculates total sequential distance for a list of lat/lon points.")]
    public static object GeoTotalDistance(
        [ExcelArgument(Name = "latlon_range", Description = "A 2D array of lat/lon points.")] object[,] latlon_range,
        [ExcelArgument(Name = "[unit]", Description = "The unit of distance: 'km' (default) or 'mi'.")] string unit = "km")
    {
        try
        {
            int pointsCount = latlon_range.GetLength(0);
            double totalDistance = 0.0;

            for (int i = 0; i < pointsCount - 1; i++)
            {
                if (GeoValidator.TryGetValidLatLon(latlon_range[i, 0], latlon_range[i, 1], out double lat1, out double lon1) &&
                    GeoValidator.TryGetValidLatLon(latlon_range[i + 1, 0], latlon_range[i + 1, 1], out double lat2, out double lon2))
                {
                    totalDistance += GeoCalculations.HaversineDistance(lat1, lon1, lat2, lon2, unit);
                }
            }

            return totalDistance;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }
}