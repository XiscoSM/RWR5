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

    /// <summary>GET PrepPedCentral/{fecha}/{pedido} — cabecera con la tienda destino y contadores.</summary>
    public Task<ApiRespuesta<PrepPedCentral>> GetAsync(DateTime fecha, int pedido, short codUsuario, bool soloConStock, CancellationToken ct = default)
        => _api.GetAsync<PrepPedCentral>(
            $"PrepPedCentral/{fecha:yyyy-MM-dd}/{pedido}?codUsuario={codUsuario}&soloConStock={soloConStock}", ct);

    /// <summary>POST .../Asignar/{codUsuario} — asigna el pedido cerrado al preparador.</summary>
    public Task<ApiRespuesta> AsignarAsync(DateTime fecha, int pedido, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync($"PrepPedCentral/{pedido}/{fecha:yyyy-MM-dd}/Asignar/{codUsuario}", cuerpo: null, ct);

    /// <summary>GET .../Linea/Siguiente — siguiente línea a preparar según recorrido por ubicación.</summary>
    public Task<ApiRespuesta<PrepPedCentralLin>> GetLineaSiguienteAsync(DateTime fecha, int pedido, short codUsuario, bool soloConStock, int codProdActual, string ubicacion, bool todos = false, CancellationToken ct = default)
        => _api.GetAsync<PrepPedCentralLin>(
            $"PrepPedCentral/{fecha:yyyy-MM-dd}/{pedido}/Linea/Siguiente?codUsuario={codUsuario}&soloConStock={soloConStock}&codProdActual={codProdActual}&ubicacion={Uri.EscapeDataString(ubicacion)}&todos={todos}", ct);

    /// <summary>GET .../Linea/Anterior — línea anterior del recorrido.</summary>
    public Task<ApiRespuesta<PrepPedCentralLin>> GetLineaAnteriorAsync(DateTime fecha, int pedido, short codUsuario, bool soloConStock, int codProdActual, string ubicacion, bool todos = false, CancellationToken ct = default)
        => _api.GetAsync<PrepPedCentralLin>(
            $"PrepPedCentral/{fecha:yyyy-MM-dd}/{pedido}/Linea/Anterior?codUsuario={codUsuario}&soloConStock={soloConStock}&codProdActual={codProdActual}&ubicacion={Uri.EscapeDataString(ubicacion)}&todos={todos}", ct);

    /// <summary>POST .../Registrar/{codUsuario} — registra lo preparado hasta ahora.</summary>
    public Task<ApiRespuesta> RegistrarAsync(DateTime fecha, int pedido, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync($"PrepPedCentral/{pedido}/{fecha:yyyy-MM-dd}/Registrar/{codUsuario}", cuerpo: null, ct);

    /// <summary>POST .../Finalizar/{codUsuario} — finaliza el pedido (estado preparado).</summary>
    public Task<ApiRespuesta> FinalizarAsync(DateTime fecha, int pedido, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync($"PrepPedCentral/{pedido}/{fecha:yyyy-MM-dd}/Finalizar/{codUsuario}", cuerpo: null, ct);
}
