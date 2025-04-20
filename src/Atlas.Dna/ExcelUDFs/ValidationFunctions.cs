using Atlas.Dna.Helpers;
using Atlas.Dna.Utilities;
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

    [ExcelFunction(Name = "GEO_ISVALID2", Description = "Checks if coordinates are valid geographic coordinates.")]
    public static bool GeoIsValid(
    [ExcelArgument(Name = "lat", Description = "The latitude value to validate.")] double lat,
    [ExcelArgument(Name = "lon", Description = "The longitude value to validate.")] double lon,
    [ExcelArgument(Name = "country", Description = "The country name or code to validate.")] string country)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return false;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return false;
            }

            var countryIndex = nearestCity.Country;
            if (countryIndex < 0 || countryIndex >= CityDataLoader.Data!.Countries.Count)
            {
                return false;
            }

            return CityDataLoader.Data.Countries[countryIndex].CountryName.Equals(country, StringComparison.OrdinalIgnoreCase) ||
                CityDataLoader.Data.Countries[countryIndex].CountryCode.Equals(country, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
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