using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Despiece y etiquetas de trazabilidad (módulo 31) contra WebApiRW (api/Despiece, aditivo).</summary>
public sealed class DespieceService
{
    private readonly ApiWeb _api;

    public DespieceService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Despiece/Lote/{prod}/{lote} — valida el producto padre + lote del EAN128.</summary>
    public Task<ApiRespuesta<DespieceLote>> GetLoteAsync(int prod, int lote, CancellationToken ct = default)
        => _api.GetAsync<DespieceLote>($"Despiece/Lote/{prod}/{lote}", ct);

    /// <summary>GET Despiece/{prod}/{lote} — despiece en componentes (filas Prod=0 = título de grupo).</summary>
    public Task<ApiRespuesta<List<DespieceLin>>> GetDespieceAsync(int prod, int lote, CancellationToken ct = default)
        => _api.GetAsync<List<DespieceLin>>($"Despiece/{prod}/{lote}", ct);

    /// <summary>POST Despiece/LoteTraspaso — crea el lote interno de una línea de traspaso; devuelve el lote.</summary>
    public Task<ApiRespuesta<int>> PostLoteTraspasoAsync(LoteTraspasoInsertDTO alta, CancellationToken ct = default)
        => _api.PostAsync<int>("Despiece/LoteTraspaso", alta, ct);

    /// <summary>GET Report/EtiqTrazabilidad/{prod}/{lote} — PDF de la etiqueta de trazabilidad.</summary>
    public Task<ApiRespuesta<byte[]>> GetEtiquetaPdfAsync(int prod, int lote, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EtiqTrazabilidad/{prod}/{lote}", ct);
}
