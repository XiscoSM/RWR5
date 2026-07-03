using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Inventarios contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class InventarioService
{
    private readonly ApiWeb _api;

    public InventarioService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Inventarios/List/{codAlm} — inventarios abiertos del almacén.</summary>
    public Task<ApiRespuesta<List<Inventario>>> GetInventariosAsync(short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<Inventario>>($"Inventarios/List/{codAlm}", ct);

    /// <summary>GET Inventarios/{codAlm}/{fecha}/Lineas — conteos del TERMINAL (el proc filtra por NumTerminal, como RWR4).</summary>
    public Task<ApiRespuesta<List<InventarioLin>>> GetLineasAsync(short codAlm, DateTime fecha, short numTerminal, short top = 200, CancellationToken ct = default)
        => _api.GetAsync<List<InventarioLin>>($"Inventarios/{codAlm}/{fecha:yyyy-MM-dd}/Lineas?codProd=0&numTerminal={numTerminal}&top={top}", ct);
}
