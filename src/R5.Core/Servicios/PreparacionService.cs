using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Preparación de pedidos de central contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class PreparacionService
{
    private readonly ApiWeb _api;

    public PreparacionService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET PrepPedCentral/List/{codAlm}?codUsuario=&amp;soloConStock= — preparaciones asignadas al usuario.</summary>
    public Task<ApiRespuesta<List<PrepPedCentral>>> GetPreparacionesAsync(short codAlm, short codUsuario, bool soloConStock, CancellationToken ct = default)
        => _api.GetAsync<List<PrepPedCentral>>($"PrepPedCentral/List/{codAlm}?codUsuario={codUsuario}&soloConStock={soloConStock}", ct);

    /// <summary>GET PrepPedCentral/{codAlm}/PedidosCerrados — pedidos cerrados pendientes de asignar.</summary>
    public Task<ApiRespuesta<List<PrepPedCentral>>> GetPedidosCerradosAsync(short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<PrepPedCentral>>($"PrepPedCentral/{codAlm}/PedidosCerrados", ct);

    /// <summary>GET .../Lineas/Siguientes — líneas pendientes de preparar (codProdActual=0 y ubicacion=0 = desde el principio).</summary>
    public Task<ApiRespuesta<List<PrepPedCentralLin>>> GetLineasPendientesAsync(DateTime fecha, int pedido, short codUsuario, bool soloConStock, CancellationToken ct = default)
        => _api.GetAsync<List<PrepPedCentralLin>>(
            $"PrepPedCentral/{fecha:yyyy-MM-dd}/{pedido}/Lineas/Siguientes?codUsuario={codUsuario}&soloConStock={soloConStock}&codProdActual=0&ubicacion=0", ct);

    /// <summary>GET .../Lineas/Preparadas — líneas ya preparadas (registrados=false: aún no registradas).</summary>
    public Task<ApiRespuesta<List<PrepPedCentralLin>>> GetLineasPreparadasAsync(DateTime fecha, int pedido, short codUsuario, bool registrados = false, CancellationToken ct = default)
        => _api.GetAsync<List<PrepPedCentralLin>>(
            $"PrepPedCentral/{fecha:yyyy-MM-dd}/{pedido}/Lineas/Preparadas?codUsuario={codUsuario}&codProdActual=0&ubicacion=0&registrados={registrados}", ct);
}
