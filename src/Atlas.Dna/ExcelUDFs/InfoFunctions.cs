using ExcelDna.Integration;
using System.Reflection;

namespace Atlas.Dna.ExcelUDFs;

public static class InfoFunctions
{
    [ExcelFunction(Name = "GEO_VERSION", Description = "Returns the Atlas Geo add-in version.")]
    public static string GetAddInVersion()
    {
        var version = Assembly.GetExecutingAssembly()
                              .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                              ?.InformationalVersion ?? "unknown";

        return version;
    }
}