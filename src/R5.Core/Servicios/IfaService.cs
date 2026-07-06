using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>
/// IFA Central de Compras (módulo 35) contra WebApiRW (api/Ifa, aditivo).
/// Fase 1: SOLO CONSULTAS — la API hace de proxy de lectura contra IFA;
/// los envíos (carga/mantenimiento) quedan para una fase posterior.
/// </summary>
public sealed class IfaService
{
    private readonly ApiWeb _api;

    public IfaService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Ifa/Pedidos?fechaDesde= — líneas de pedido en IFA con fecha >= la dada.</summary>
    public Task<ApiRespuesta<List<IfaPedidoLin>>> GetPedidosAsync(DateTime fechaDesde, CancellationToken ct = default)
        => _api.GetAsync<List<IfaPedidoLin>>($"Ifa/Pedidos?fechaDesde={fechaDesde:yyyy-MM-dd}", ct);

    /// <summary>GET Ifa/Recepciones?fechaDesde=&amp;fechaHasta= — líneas de recepción en IFA en el rango.</summary>
    public Task<ApiRespuesta<List<IfaRecepcionLin>>> GetRecepcionesAsync(DateTime fechaDesde, DateTime fechaHasta, CancellationToken ct = default)
        => _api.GetAsync<List<IfaRecepcionLin>>($"Ifa/Recepciones?fechaDesde={fechaDesde:yyyy-MM-dd}&fechaHasta={fechaHasta:yyyy-MM-dd}", ct);
}
