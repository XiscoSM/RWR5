using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Preparación de pedidos WEB contra WebApiRW (api/PreparacionWww, canon R4).
/// Módulo que en R3/R4 preparaba los pedidos de la tienda online.</summary>
public sealed class PreparacionWwwService
{
    private readonly ApiWeb _api;

    public PreparacionWwwService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET PreparacionWww/List/{codAlm}?codUsuario= — pedidos web a preparar.</summary>
    public Task<ApiRespuesta<List<CabPreparacionWww>>> GetListaAsync(short codAlm, short codUsuario, CancellationToken ct = default)
        => _api.GetAsync<List<CabPreparacionWww>>($"PreparacionWww/List/{codAlm}?codUsuario={codUsuario}", ct);

    /// <summary>GET PreparacionWww/{pedido}/{codAlm}/{fecha} — cabecera con contadores.</summary>
    public Task<ApiRespuesta<CabPreparacionWww>> GetAsync(int pedido, short codAlm, DateTime fecha, CancellationToken ct = default)
        => _api.GetAsync<CabPreparacionWww>($"PreparacionWww/{pedido}/{codAlm}/{fecha:yyyy-MM-dd}", ct);

    /// <summary>GET .../Lineas/Siguientes — líneas pendientes del recorrido.</summary>
    public Task<ApiRespuesta<List<LinPreparacionWww>>> GetLineasPendientesAsync(int pedido, DateTime fecha, short codAlm, short codUsuario, CancellationToken ct = default)
        => _api.GetAsync<List<LinPreparacionWww>>($"PreparacionWww/{pedido}/{fecha:yyyy-MM-dd}/Lineas/Siguientes?codAlm={codAlm}&codUsuario={codUsuario}&codProdActual=0&ubicacion=0", ct);

    /// <summary>GET .../Lineas/Preparadas — líneas ya preparadas.</summary>
    public Task<ApiRespuesta<List<LinPreparacionWww>>> GetLineasPreparadasAsync(int pedido, short codAlm, DateTime fecha, short codUsuario, CancellationToken ct = default)
        => _api.GetAsync<List<LinPreparacionWww>>($"PreparacionWww/{pedido}/{codAlm}/{fecha:yyyy-MM-dd}/Lineas/Preparadas?codUsuario={codUsuario}&codProdActual=0&ubicacion=0", ct);

    /// <summary>GET .../Linea/Siguiente — siguiente línea del recorrido por ubicación.</summary>
    public Task<ApiRespuesta<LinPreparacionWww>> GetLineaSiguienteAsync(int pedido, short codAlm, DateTime fecha, short codUsuario, int codProdActual, int ubicacion, bool todos = false, CancellationToken ct = default)
        => _api.GetAsync<LinPreparacionWww>($"PreparacionWww/{pedido}/{codAlm}/{fecha:yyyy-MM-dd}/Linea/Siguiente?codUsuario={codUsuario}&codProdActual={codProdActual}&ubicacion={ubicacion}&todos={todos}", ct);

    /// <summary>GET .../Linea/Anterior — línea anterior del recorrido.</summary>
    public Task<ApiRespuesta<LinPreparacionWww>> GetLineaAnteriorAsync(int pedido, short codAlm, DateTime fecha, short codUsuario, int codProdActual, int ubicacion, bool todos = false, CancellationToken ct = default)
        => _api.GetAsync<LinPreparacionWww>($"PreparacionWww/{pedido}/{codAlm}/{fecha:yyyy-MM-dd}/Linea/Anterior?codUsuario={codUsuario}&codProdActual={codProdActual}&ubicacion={ubicacion}&todos={todos}", ct);

    /// <summary>POST .../Asignar/{codUsuario} — asigna el pedido web al preparador.</summary>
    public Task<ApiRespuesta> AsignarAsync(int pedido, short codAlm, DateTime fecha, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync($"PreparacionWww/{pedido}/{codAlm}/{fecha:yyyy-MM-dd}/Asignar/{codUsuario}", cuerpo: null, ct);

    /// <summary>POST .../{codProd}/CantidadPreparada/{cantPrep} — graba lo preparado de una línea.</summary>
    public Task<ApiRespuesta> PostCantidadPreparadaAsync(int pedido, short codAlm, DateTime fecha, int codProd, decimal cantPrep, CancellationToken ct = default)
        => _api.PostAsync($"PreparacionWww/{pedido}/{codAlm}/{fecha:yyyy-MM-dd}/{codProd}/CantidadPreparada/{cantPrep}", cuerpo: null, ct);

    /// <summary>POST .../Registrar/{codUsuario} — registra la preparación del pedido web.</summary>
    public Task<ApiRespuesta> RegistrarAsync(int pedido, DateTime fecha, short codAlm, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync($"PreparacionWww/{pedido}/{fecha:yyyy-MM-dd}/{codAlm}/Registrar/{codUsuario}", cuerpo: null, ct);

    /// <summary>GET .../Producto/{codProd} — línea de un producto concreto del pedido.</summary>
    public Task<ApiRespuesta<LinPreparacionWww>> GetProductoAsync(int pedido, DateTime fecha, int codProd, CancellationToken ct = default)
        => _api.GetAsync<LinPreparacionWww>($"PreparacionWww/{pedido}/{fecha:yyyy-MM-dd}/Producto/{codProd}", ct);
}
