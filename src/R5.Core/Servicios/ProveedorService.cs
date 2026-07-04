using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Proveedores y gamas de compra contra WebApiRW (canon R4).</summary>
public sealed class ProveedorService
{
    private readonly ApiWeb _api;

    public ProveedorService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Proveedores/Gamas?codEan= — proveedores/gamas de un producto (identificar proveedor escaneando).</summary>
    public Task<ApiRespuesta<List<Proveedor>>> GetGamasPorEanAsync(long codEan, CancellationToken ct = default)
        => _api.GetAsync<List<Proveedor>>($"Proveedores/Gamas?codEan={codEan}", ct);

    /// <summary>GET Proveedores/{codProv}/Gamas — gamas de un proveedor.</summary>
    public Task<ApiRespuesta<List<Proveedor>>> GetGamasPorProvAsync(int codProv, CancellationToken ct = default)
        => _api.GetAsync<List<Proveedor>>($"Proveedores/{codProv}/Gamas", ct);
}
