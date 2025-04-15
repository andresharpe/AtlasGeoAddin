using Atlas.Dna.Helpers;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class ConversionFunctions
{
    [ExcelFunction(Name = "GEO_TO_GOOGLE_MAPS_URL", Description = "Converts geographic coordinates to a Google Maps URL.")]
    public static object GeoLatLonToGoogleMapsUrl(
        [ExcelArgument(Name = "lat", Description = "The latitude of the point.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude of the point.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }
            return $"https://www.google.com/maps?q={lat},{lon}";
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }
}