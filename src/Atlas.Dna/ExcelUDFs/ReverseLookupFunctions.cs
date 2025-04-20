using Atlas.Dna.Helpers;
using Atlas.Dna.Utilities;
using ExcelDna.Integration;

namespace Atlas.Dna.ExcelUDFs;

public static class ReverseLookupFunctions
{
    [ExcelFunction(Name = "GEO_COUNTRY", Description = "Finds the country name for a given latitude and longitude.")]
    public static object GeoReverseLookupCountry(
        [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var countryIndex = nearestCity.Country;
            if (countryIndex < 0 || countryIndex >= CityDataLoader.Data!.Countries.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.Countries[countryIndex].CountryName;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_COUNTRYCODE", Description = "Finds the country ISO2 code for a given latitude and longitude.")]
    public static object GeoReverseLookupCountryCode(
    [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
    [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var countryIndex = nearestCity.Country;
            if (countryIndex < 0 || countryIndex >= CityDataLoader.Data!.Countries.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.Countries[countryIndex].CountryCode;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_ADMIN", Description = "Finds the administration name for a given latitude and longitude.")]
    public static object GeoReverseLookupAdminName(
    [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
    [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var adminIndex = nearestCity.Admin;
            if (adminIndex < 0 || adminIndex >= CityDataLoader.Data!.Admins.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.Admins[adminIndex].AdminName;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_ADMINCODE", Description = "Finds the administration code for a given latitude and longitude.")]
    public static object GeoReverseLookupAdminCode(
        [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var adminIndex = nearestCity.Admin;
            if (adminIndex < 0 || adminIndex >= CityDataLoader.Data!.Admins.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.Admins[adminIndex].AdminCode;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_ADMINTYPE", Description = "Finds the administration type for a given latitude and longitude.")]
    public static object GeoReverseLookupAdminType(
        [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var adminIndex = nearestCity.Admin;
            if (adminIndex < 0 || adminIndex >= CityDataLoader.Data!.Admins.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.Admins[adminIndex].AdminType;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }


    [ExcelFunction(Name = "GEO_CITY", Description = "Finds the city name for a given latitude and longitude.")]
    public static object GeoReverseLookupCity(
        [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var cityIndex = nearestCity.City;
            if (cityIndex < 0 || cityIndex >= CityDataLoader.Data!.CityStringPool.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.CityStringPool[nearestCity.City];
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_TIMEZONE", Description = "Finds the Timezone name for a given latitude and longitude.")]
    public static object GeoReverseLookupTimeZone(
        [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var tzIndex = nearestCity.Timezone;
            if (tzIndex < 0 || tzIndex >= CityDataLoader.Data!.TimeZoneStringPool.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            return CityDataLoader.Data.TimeZoneStringPool[tzIndex];
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_TIMEZONEOFFSET_STANDARD", Description = "Finds the standard Timezone offset for a given latitude and longitude.")]
    public static object GeoReverseLookupTimeZoneOffsetStandard(
        [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
        [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var tzIndex = nearestCity.Timezone;
            if (tzIndex < 0 || tzIndex >= CityDataLoader.Data!.TimeZoneStringPool.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            var tz = CityDataLoader.Data.TimeZoneStringPool[tzIndex];

            return tz.ToTimeZoneOffsets().StandardOffset;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }

    [ExcelFunction(Name = "GEO_TIMEZONEOFFSET_DAYLIGHTSAVINGS", Description = "Finds the day light savings Timezone offset for a given latitude and longitude.")]
    public static object GeoReverseLookupTimeZoneOffsetDls(
    [ExcelArgument(Name = "lat", Description = "The latitude value to lookup.")] double lat,
    [ExcelArgument(Name = "lon", Description = "The longitude value to lookup.")] double lon)
    {
        try
        {
            if (!GeoValidator.IsValidLatLon(lat, lon))
            {
                return ExcelError.ExcelErrorValue;
            }

            if (!CityDataLoader.IsLoaded)
            {
                CityDataLoader.Load();
            }

            var nearestCity = CityDataLoader.GetNearestCity((float)lat, (float)lon);
            if (nearestCity == null)
            {
                return ExcelError.ExcelErrorNA;
            }

            var tzIndex = nearestCity.Timezone;
            if (tzIndex < 0 || tzIndex >= CityDataLoader.Data!.TimeZoneStringPool.Count)
            {
                return ExcelError.ExcelErrorNA;
            }

            var tz = CityDataLoader.Data.TimeZoneStringPool[tzIndex];

            return tz.ToTimeZoneOffsets().DaylightSavingOffset;
        }
        catch
        {
            return ExcelError.ExcelErrorValue;
        }
    }
}