namespace Atlas.Dna;

using ExcelDna.Integration;

public static partial class GeoUdf
{
    /// <summary>
    /// Validates the latitude and longitude values.
    /// </summary>
    /// <param name="lat">The latitude value to validate.</param>
    /// <param name="lon">The longitude value to validate.</param>
    /// <returns>True if both latitude and longitude are valid; otherwise, false.</returns>
    [ExcelFunction(Description = "Validates the latitude and longitude values.")]
    public static bool GeoValidateLatLon(
        [ExcelArgument(Name = "lat", Description = "The latitude value to validate.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to validate.")] double lon)
    {
        return IsValidLatLon(lat, lon);
    }

    private static bool IsValidLatLon(double lat, double lon) =>
        lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;
}