namespace Atlas.Dna.Helpers;

/// <summary>
/// Provides geographical calculations such as distance, bearing, midpoint, and bounding box.
/// </summary>
public static class GeoCalculations
{
    private const double _earthRadiusKm = 6371.0;
    private const double _earthRadiusMi = 3958.8;
    private const double _degreesToRadiansFactor = Math.PI / 180.0f;
    private const double _radiansToDegreesFactor = 180.0f / Math.PI;

    /// <summary>
    /// Calculates the Haversine distance between two points specified by latitude/longitude.
    /// </summary>
    /// <param name="lat1">Latitude of the first point.</param>
    /// <param name="lon1">Longitude of the first point.</param>
    /// <param name="lat2">Latitude of the second point.</param>
    /// <param name="lon2">Longitude of the second point.</param>
    /// <param name="unit">Unit of distance ("km" for kilometers, "mi" for miles).</param>
    /// <returns>The distance between the two points in the specified unit.</returns>
    public static double HaversineDistance(double lat1, double lon1, double lat2, double lon2, string unit = "km")
    {
        double dLat = ToRad(lat2 - lat1);
        double dLon = ToRad(lon2 - lon1);

        lat1 = ToRad(lat1);
        lat2 = ToRad(lat2);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return (unit == "mi") ? _earthRadiusMi * c : _earthRadiusKm * c;
    }

    /// <summary>
    /// Calculates the initial bearing (forward azimuth) between two points specified by latitude/longitude.
    /// </summary>
    /// <param name="lat1">Latitude of the first point.</param>
    /// <param name="lon1">Longitude of the first point.</param>
    /// <param name="lat2">Latitude of the second point.</param>
    /// <param name="lon2">Longitude of the second point.</param>
    /// <returns>The initial bearing in degrees from the first point to the second point.</returns>
    public static double InitialBearing(double lat1, double lon1, double lat2, double lon2)
    {
        lat1 = ToRad(lat1);
        lat2 = ToRad(lat2);
        double dLon = ToRad(lon2 - lon1);

        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

        double bearing = Math.Atan2(y, x);
        return (ToDeg(bearing) + 360) % 360;
    }

    /// <summary>
    /// Calculates the midpoint between two points specified by latitude/longitude.
    /// </summary>
    /// <param name="lat1">Latitude of the first point.</param>
    /// <param name="lon1">Longitude of the first point.</param>
    /// <param name="lat2">Latitude of the second point.</param>
    /// <param name="lon2">Longitude of the second point.</param>
    /// <returns>A tuple containing the latitude and longitude of the midpoint.</returns>
    public static (double lat, double lon) Midpoint(double lat1, double lon1, double lat2, double lon2)
    {
        lat1 = ToRad(lat1);
        lon1 = ToRad(lon1);
        lat2 = ToRad(lat2);
        double dLon = ToRad(lon2 - lon1);

        double Bx = Math.Cos(lat2) * Math.Cos(dLon);
        double By = Math.Cos(lat2) * Math.Sin(dLon);

        double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
        double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);

        return (ToDeg(lat3), ToDeg(lon3));
    }

    /// <summary>
    /// Calculates a bounding box around a point specified by latitude/longitude and a radius in kilometers.
    /// </summary>
    /// <param name="lon">Longitude of the center point.</param>
    /// <param name="lat">Latitude of the center point.</param>
    /// <param name="radiusInKm">Radius in kilometers.</param>
    /// <returns>A BoundingBox object representing the bounding box.</returns>
    public static BoundingBox GetBoundingBox(double lon, double lat, double radiusInKm)
    {
        var radiusRatio = radiusInKm / _earthRadiusKm;
        var latOffset = radiusRatio * _radiansToDegreesFactor;
        var lonOffset = Math.Asin(radiusRatio) / Math.Cos(lat * _degreesToRadiansFactor) * _radiansToDegreesFactor;

        return new BoundingBox
        (
            MaxLat: lat + latOffset,
            MinLat: lat - latOffset,
            MaxLon: lon + lonOffset,
            MinLon: lon - lonOffset
        );
    }

    private static double ToRad(double angle) => Math.PI * angle / 180.0;

    private static double ToDeg(double angle) => angle * 180.0 / Math.PI;

    /// <summary>
    /// Represents a bounding box defined by maximum and minimum latitudes and longitudes.
    /// </summary>
    /// <param name="MaxLon">Maximum longitude.</param>
    /// <param name="MinLon">Minimum longitude.</param>
    /// <param name="MaxLat">Maximum latitude.</param>
    /// <param name="MinLat">Minimum latitude.</param>
    public record BoundingBox(double MaxLon, double MinLon, double MaxLat, double MinLat)
    {
        /// <summary>
        /// Determines whether a point specified by latitude/longitude is within the bounding box.
        /// </summary>
        /// <param name="lon">Longitude of the point.</param>
        /// <param name="lat">Latitude of the point.</param>
        /// <returns>True if the point is within the bounding box, otherwise false.</returns>
        public bool Contains(double lon, double lat)
        {
            return lon > MinLon && lon < MaxLon && lat > MinLat && lat < MaxLat;
        }
    };
}