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

    /// <summary>GET Report/Traspaso/{num}/{fecha} — PDF del documento de traspaso.</summary>
    public Task<ApiRespuesta<byte[]>> GetTraspasoPdfAsync(int numTraspaso, DateTime fecha, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/Traspaso/{numTraspaso}/{fecha:yyyy-MM-dd}", ct);

    /// <summary>GET Report/PedidoCentral/{num}/{fecha} — PDF del documento de pedido a central.</summary>
    public Task<ApiRespuesta<byte[]>> GetPedidoCentralPdfAsync(int numPedido, DateTime fecha, byte estado, short almCentral, bool vertical = false, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/PedidoCentral/{numPedido}/{fecha:yyyy-MM-dd}?estado={estado}&almCentral={almCentral}&vertical={vertical}", ct);
}

/// <summary>Abre un archivo en el visor de la plataforma (impl. en el host MAUI).</summary>
public interface IVisorArchivos
{
    /// <summary>Guarda el contenido en caché y lo abre con la app asociada (visor PDF).</summary>
    Task AbrirAsync(string nombreArchivo, byte[] contenido);
}
