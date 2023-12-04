using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Aptabase;

internal class SystemInfo
{
    private static string _pkgVersion = typeof(AptabaseClient).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

    public bool IsDebug { get; }
    public string OsName { get; }
	public string OsVersion { get; }
	public string SdkVersion { get; }
    public string Locale { get; }
    public string AppVersion { get; }
    public string AppBuildNumber { get; }

    public SystemInfo(Assembly? assembly)
	{
        this.IsDebug = IsInDebugMode(assembly);
        this.OsName = GetOsName();
        this.OsVersion = GetOsVersion();
        this.SdkVersion = $"Kingas.Aptabase@{_pkgVersion}";
        this.Locale = Thread.CurrentThread.CurrentCulture.Name;
        var appVersion = Assembly.GetEntryAssembly()?.GetName().Version;
        this.AppVersion = appVersion?.ToString() ?? string.Empty;
        this.AppBuildNumber = (appVersion?.Build ?? 0).ToString();
    }
        
    private static bool IsInDebugMode(Assembly? assembly)
    {
        if (assembly == null)
            return false;

        var attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), false);
        if (attributes.Length > 0)
        {
            var debuggable = attributes[0] as DebuggableAttribute;
            if (debuggable != null)
                return (debuggable.DebuggingFlags & DebuggableAttribute.DebuggingModes.Default) == DebuggableAttribute.DebuggingModes.Default;
            else
                return false;
        }
        else
            return false;
    }

    private string GetOsName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "Windows";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "macOS";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "Linux";

        return string.Empty;
    }

    private string GetOsVersion()
    {
        return Environment.OSVersion.VersionString;
    }
}

