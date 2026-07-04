namespace R5.Core.Config;

/// <summary>Identidad de la propia app (versión del paquete); la aporta el host MAUI.</summary>
public sealed class InfoApp
{
    public string Version { get; }

    public InfoApp(string version) => Version = version;
}
