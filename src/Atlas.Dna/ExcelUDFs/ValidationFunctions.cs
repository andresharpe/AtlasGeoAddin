using Atlas.Dna.Helpers;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class ValidationFunctions
{
    [ExcelFunction(Name = "GEO_ISVALID", Description = "Checks if coordinates are valid geographic coordinates.")]
    public static bool GeoIsValid(
        [ExcelArgument(Name = "lat", Description = "The latitude value to validate.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to validate.")] double lon)
    {
        return GeoValidator.IsValidLatLon(lat, lon);
    }

    [ExcelFunction(Name = "GEO_NORMALIZE", Description = "Normalizes latitude and longitude to standard ranges.")]
    public static object[,] GeoNormalize(
        [ExcelArgument(Name = "lat", Description = "The latitude value to normalize.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to normalize.")] double lon)
    {
        double normalizedLat = ((lat + 90) % 180 + 180) % 180 - 90;
        double normalizedLon = ((lon + 180) % 360 + 360) % 360 - 180;

        return new object[,] { { normalizedLat, normalizedLon } };
    }
}