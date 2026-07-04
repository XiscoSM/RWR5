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

    /// <summary>GET Inventarios/{alm}/{fecha}/Prod?codEan= — producto a contar, con su total ya contado.</summary>
    public Task<ApiRespuesta<InventarioLin>> GetProdPorEanAsync(short codAlm, DateTime fecha, long codEan, CancellationToken ct = default)
        => _api.GetAsync<InventarioLin>($"Inventarios/{codAlm}/{fecha:yyyy-MM-dd}/Prod?codEan={codEan}", ct);

    /// <summary>GET Inventarios/{alm}/{fecha}/Prod/{codProd} — producto a contar, por código.</summary>
    public Task<ApiRespuesta<InventarioLin>> GetProdPorCodAsync(short codAlm, DateTime fecha, int codProd, CancellationToken ct = default)
        => _api.GetAsync<InventarioLin>($"Inventarios/{codAlm}/{fecha:yyyy-MM-dd}/Prod/{codProd}", ct);

    /// <summary>
    /// POST Inventarios/{alm}/{fecha}/{prod} — registra un conteo (se SUMA a lo ya contado).
    /// Respuesta sin tipar: el cuerpo que devuelve el proc no es un InventarioLin utilizable.
    /// </summary>
    public Task<ApiRespuesta> PostConteoAsync(InvLinInsertDTO conteo, CancellationToken ct = default)
        => _api.PostAsync($"Inventarios/{conteo.CodAlm}/{conteo.Fecha:yyyy-MM-dd}/{conteo.CodProd}", conteo, ct);
}
