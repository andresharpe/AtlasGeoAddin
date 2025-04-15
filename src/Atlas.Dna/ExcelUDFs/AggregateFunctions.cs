using Atlas.Dna.Helpers;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class AggregateFunctions
{
    [ExcelFunction(Name = "GEO_CENTROID", Description = "Calculates the centroid of a range of lat/lon entries.")]
    public static object GeoCentroid(
        [ExcelArgument(Name = "lat_range", Description = "A 2D array of latitudes to calculate the centroid.")] object[,] lat_range,
        [ExcelArgument(Name = "lon_range", Description = "A 2D array of longitudes to calculate the centroid.")] object[,] lon_range)
    {
        try
        {
            int latsCount = lat_range.GetLength(0);
            int lonsCount = lon_range.GetLength(0);

            if (latsCount != lonsCount)
            {
                return ExcelError.ExcelErrorValue;
            }

            double sumLat = 0;
            double sumLon = 0;
            int validCount = 0;

            for (int i = 0; i < latsCount; i++)
            {
                if (GeoValidator.TryGetValidLatLon(lat_range[i, 0], lon_range[i, 0], out double validLat, out double validLon))
                {
                    sumLat += validLat;
                    sumLon += validLon;
                    validCount++;
                }
            }

            if (validCount == 0)
            {
                return ExcelError.ExcelErrorValue;
            }

            double centroidLat = sumLat / validCount;
            double centroidLon = sumLon / validCount;

            return new object[] { centroidLat, centroidLon };
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_DIRECTION", Description = "Calculates the compass direction (N, NE, SW, etc.) between two points specified by latitude/longitude.")]
    public static object GeoDirection(
        [ExcelArgument(Name = "lat1", Description = "Latitude of the first point.")] double lat1,
        [ExcelArgument(Name = "lon1", Description = "Longitude of the first point.")] double lon1,
        [ExcelArgument(Name = "lat2", Description = "Latitude of the second point.")] double lat2,
        [ExcelArgument(Name = "lon2", Description = "Longitude of the second point.")] double lon2)
    {
        try
        {
            double bearing = GeoCalculations.InitialBearing(lat1, lon1, lat2, lon2);
            string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            int index = (int)Math.Round(((bearing % 360) / 22.5), MidpointRounding.AwayFromZero) % 16;
            return directions[index];
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_MIDPOINT", Description = "Calculates the midpoint between two geographic coordinates.")]
    public static object GeoMidpoint(
   [ExcelArgument(Name = "lat1", Description = "The latitude of the first coordinate.")] double lat1,
   [ExcelArgument(Name = "lon1", Description = "The longitude of the first coordinate.")] double lon1,
   [ExcelArgument(Name = "lat2", Description = "The latitude of the second coordinate.")] double lat2,
   [ExcelArgument(Name = "lon2", Description = "The longitude of the second coordinate.")] double lon2)
    {
        try
        {
            var (midLat, midLon) = GeoCalculations.Midpoint(lat1, lon1, lat2, lon2);
            return new object[,] { { midLat, midLon } };
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_BEARING", Description = "Calculates the initial bearing between two geographic coordinates.")]
    public static object GeoBearing(
        [ExcelArgument(Name = "lat1", Description = "The latitude of the first coordinate.")] double lat1,
        [ExcelArgument(Name = "lon1", Description = "The longitude of the first coordinate.")] double lon1,
        [ExcelArgument(Name = "lat2", Description = "The latitude of the second coordinate.")] double lat2,
        [ExcelArgument(Name = "lon2", Description = "The longitude of the second coordinate.")] double lon2)
    {
        try
        {
            return GeoCalculations.InitialBearing(lat1, lon1, lat2, lon2);
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }
}