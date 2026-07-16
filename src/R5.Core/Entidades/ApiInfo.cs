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
    /// <summary>Tri-estado real (R3): 0 = no se pide link al registrar, 1 = opcional
    /// (se admite «0»), 2 = obligatorio. Con APIs antiguas que aún no lo envían,
    /// se cae al bool (true → obligatorio, comportamiento previo).</summary>
    public byte? LinkPdfAlbaranNivel { get; set; }
    public byte NivelLink => LinkPdfAlbaranNivel ?? (LinkPdfAlbaran ? (byte)2 : (byte)0);
    public string? ServIIS { get; set; }
}
