using KdTree;
using KdTree.Math;
using System.Collections.Concurrent;

namespace Atlas.Dna.Helpers;

public static class GeoIndexManager
{
    // Thread-safe dictionary to hold indexes by name
    private static readonly ConcurrentDictionary<string, KdTree<double, object>> Indexes = new();

    private static readonly object Locker = new();

    /// <summary>
    /// Creates or rebuilds a spatial index.
    /// </summary>
    /// <param name="indexName">The name of the index.</param>
    /// <param name="ids">The collection of IDs corresponding to the geographic points.</param>
    /// <param name="lats">The collection of latitudes.</param>
    /// <param name="lons">The collection of longitudes.</param>
    /// <exception cref="ArgumentException">Thrown when the index name is null or empty, or when the lengths of the provided collections do not match.</exception>
    public static bool CreateIndex(string indexName, IEnumerable<object> ids, IEnumerable<double> lats, IEnumerable<double> lons)
    {
        if (string.IsNullOrEmpty(indexName))
            throw new ArgumentException("Index name cannot be null or empty.", nameof(indexName));

        var idArray = ids.ToArray();
        var latArray = lats.ToArray();
        var lonArray = lons.ToArray();

        if (idArray.Length != latArray.Length || idArray.Length != lonArray.Length)
            throw new ArgumentException("The length of IDs, latitudes, and longitudes must be identical.");

        var tree = new KdTree<double, object>(2, new GeoMath());

        for (int i = 0; i < idArray.Length; i++)
        {
            tree.Add(new[] { latArray[i], lonArray[i] }, idArray[i]);
        }

        lock (Locker)
        {
            Indexes[indexName] = tree;
        }
        return true;
    }

    /// <summary>
    /// Clears a spatial index.
    /// </summary>
    /// <param name="indexName">The name of the index to clear.</param>
    public static void ClearIndex(string indexName)
    {
        lock (Locker)
        {
            Indexes.TryRemove(indexName, out _);
        }
    }

    /// <summary>
    /// Finds the nearest neighbors to the given point.
    /// </summary>
    /// <param name="indexName">The name of the index to search.</param>
    /// <param name="lat">The latitude of the point to search from.</param>
    /// <param name="lon">The longitude of the point to search from.</param>
    /// <param name="k">The number of nearest neighbors to find.</param>
    /// <param name="unit">The unit of distance ("km" or "mi").</param>
    /// <returns>A list of tuples containing the ID and distance of each nearest neighbor.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the specified index does not exist.</exception>
    public static List<(object Id, double Distance)> FindNearest(string indexName, double lat, double lon, int k = 1, string unit = "km")
    {
        if (!Indexes.TryGetValue(indexName, out var tree))
            throw new InvalidOperationException($"Index '{indexName}' does not exist.");

        var results = tree.GetNearestNeighbours(new[] { lat, lon }, k);

        return results.Select(node => (node.Value, GeoCalculations.HaversineDistance(lat, lon, node.Point[0], node.Point[1], unit))).ToList();
    }

    /// <summary>
    /// Custom KD-Tree math using geographic (lat/lon) coordinates.
    /// </summary>
    private class GeoMath : TypeMath<double>
    {
        public override double DistanceSquaredBetweenPoints(double[] a, double[] b)
        {
            // Approximate distance squared in degrees (good enough for KD-tree sorting)
            double latDiff = a[0] - b[0];
            double lonDiff = a[1] - b[1];
            return latDiff * latDiff + lonDiff * lonDiff;
        }

        public override int Compare(double a, double b)
        {
            return a.CompareTo(b);
        }

        public override double MinValue => double.MinValue;
        public override double MaxValue => double.MaxValue;

        public override bool AreEqual(double a, double b)
        {
            return Math.Abs(a - b) < 1e-10;
        }

        public override double Zero => 0;
        public override double NegativeInfinity => double.NegativeInfinity;
        public override double PositiveInfinity => double.PositiveInfinity;

        public override double Add(double a, double b)
        {
            return a + b;
        }

        public override double Subtract(double a, double b)
        {
            return a - b;
        }

        public override double Multiply(double a, double b)
        {
            return a * b;
        }
    }
}