namespace Atlas.Dna.Helpers;

public class GeoValidator
{
    public static bool IsValidLatLon(double lat, double lon) => lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;

    public static bool TryGetValidLatLon(object latObj, object lonObj, out double lat, out double lon)
    {
        lat = ParseToDouble(latObj);
        lon = ParseToDouble(lonObj);

        return IsValidLatLon(lat, lon);
    }

    private static double ParseToDouble(object obj)
    {
        return obj switch
        {
            double d => d,
            string s when double.TryParse(s, out var result) => result,
            _ => -1
        };
    }
}