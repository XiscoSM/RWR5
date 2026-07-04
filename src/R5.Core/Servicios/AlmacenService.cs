using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Consulta de almacenes contra WebApiRW (canon R4).</summary>
public sealed class AlmacenService
{
    private readonly ApiWeb _api;

    public AlmacenService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Almacen/{codAlm} — datos del almacén (CodAlm 0 = no existe).</summary>
    public Task<ApiRespuesta<Almacen>> GetAsync(short codAlm, CancellationToken ct = default)
        => _api.GetAsync<Almacen>($"Almacen/{codAlm}", ct);
}
