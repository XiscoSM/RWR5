namespace R5.Core.Entidades;

/// <summary>
/// Handshake con la API (GET api/ApiInfo, endpoint anónimo). Se usa en Ajustes
/// para verificar URL/conectividad antes de enrolar y para mostrar el entorno real.
/// </summary>
public sealed class ApiInfo
{
    public string Servidor { get; set; } = "";
    public string BaseDatos { get; set; } = "";
    public bool Desarrollo { get; set; }
    public string Empresa { get; set; } = "";
    public DateTime FechaHora { get; set; }
    public string Version { get; set; } = "";
    public string ApiName { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string Protocol { get; set; } = "";
    public string IpCliente { get; set; } = "";
    public string? Result { get; set; } = "";
    public bool LinkPdfAlbaran { get; set; }
    public string? ServIIS { get; set; }
}
