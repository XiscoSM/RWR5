using R5.Core.Api;

namespace R5.Core.Servicios;

/// <summary>
/// Informes RDLC renderizados en WebApiRW (endpoint api/Report → PDF).
/// El cliente solo recibe y muestra el PDF, igual en Windows y Android.
/// </summary>
public sealed class ReportService
{
    private readonly ApiWeb _api;

    public ReportService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Report/Traspaso/{num}/{fecha} — PDF del documento de traspaso.
    /// verCostes/detallado = variantes del RDLC de R3 (informe detallado con costes).</summary>
    public Task<ApiRespuesta<byte[]>> GetTraspasoPdfAsync(int numTraspaso, DateTime fecha, bool verCostes = false, bool detallado = false, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/Traspaso/{numTraspaso}/{fecha:yyyy-MM-dd}?verCostes={verCostes}&detallado={detallado}", ct);

    /// <summary>GET Report/PedidoCentral/{num}/{fecha} — PDF del documento de pedido a central.
    /// vertical = rdlc vertical; saltoPasillo = salto de página por pasillo; conCantEnv = cantidad por envase.</summary>
    public Task<ApiRespuesta<byte[]>> GetPedidoCentralPdfAsync(int numPedido, DateTime fecha, byte estado, short almCentral, bool vertical = false, bool saltoPasillo = false, bool conCantEnv = false, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/PedidoCentral/{numPedido}/{fecha:yyyy-MM-dd}?estado={estado}&almCentral={almCentral}&vertical={vertical}&saltoPasillo={saltoPasillo}&conCantEnv={conCantEnv}", ct);

    /// <summary>GET Report/PedidoCentralGama/{gama}/{fecha} — PDF de pedidos a central agrupados por gama.</summary>
    public Task<ApiRespuesta<byte[]>> GetPedidoCentralGamaPdfAsync(int gama, DateTime fecha, byte estado, short almCentral, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/PedidoCentralGama/{gama}/{fecha:yyyy-MM-dd}?estado={estado}&almCentral={almCentral}", ct);

    /// <summary>GET Report/InformeManualAgrup/{tipo}/{fecha} — PDF de informes manuales agrupados.</summary>
    public Task<ApiRespuesta<byte[]>> GetInformeAgrupPdfAsync(byte tipo, DateTime fecha, short almDest, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/InformeManualAgrup/{tipo}/{fecha:yyyy-MM-dd}?almDest={almDest}", ct);
}

/// <summary>Abre un archivo en el visor de la plataforma (impl. en el host MAUI).</summary>
public interface IVisorArchivos
{
    /// <summary>Guarda el contenido en caché y lo abre con la app asociada (visor PDF).</summary>
    Task AbrirAsync(string nombreArchivo, byte[] contenido);
}
