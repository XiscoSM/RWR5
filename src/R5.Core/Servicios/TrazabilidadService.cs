using R5.Core.Api;

namespace R5.Core.Servicios;

/// <summary>
/// Trazabilidad de lotes (visor del módulo 7 de R3): movimientos de un lote con
/// niveles de detalle y navegación padre/hijo. Las filas llegan dinámicas
/// (columna → texto) porque el result set del proc cambia según el detalle.
/// </summary>
public sealed class TrazabilidadService
{
    private readonly ApiWeb _api;

    public TrazabilidadService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET ProdLote/Movs/{prod}/{lote}?alm=&amp;detalle= — movimientos del lote
    /// (prod 0 = buscar solo por lote; detalle 0/1/2 como los radios de R3).</summary>
    public Task<ApiRespuesta<List<Dictionary<string, string?>>>> GetMovsAsync(int prod, int lote, short codAlm, byte detalle, CancellationToken ct = default)
        => _api.GetAsync<List<Dictionary<string, string?>>>($"ProdLote/Movs/{prod}/{lote}?alm={codAlm}&detalle={detalle}", ct);

    /// <summary>GET Report/EtiqTrazabilidad/{prod}/{lote} — PDF de la etiqueta del lote.</summary>
    public Task<ApiRespuesta<byte[]>> GetEtiquetaPdfAsync(int prod, int lote, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EtiqTrazabilidad/{prod}/{lote}", ct);
}
