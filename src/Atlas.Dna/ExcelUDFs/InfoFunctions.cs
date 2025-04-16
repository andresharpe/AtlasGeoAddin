using ExcelDna.Integration;
using System.Reflection;

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