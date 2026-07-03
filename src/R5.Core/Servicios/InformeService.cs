using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Informes manuales contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class InformeService
{
    private readonly ApiWeb _api;

    public InformeService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Informe/List/{codAlm}?codUsuario= — informes del usuario en el almacén.</summary>
    public Task<ApiRespuesta<List<Informe>>> GetInformesAsync(short codAlm, short codUsuario, CancellationToken ct = default)
        => _api.GetAsync<List<Informe>>($"Informe/List/{codAlm}?codUsuario={codUsuario}", ct);

    /// <summary>GET Informe/{num}/Lineas — líneas del informe.</summary>
    public Task<ApiRespuesta<List<InformeLin>>> GetLineasAsync(int numInforme, CancellationToken ct = default)
        => _api.GetAsync<List<InformeLin>>($"Informe/{numInforme}/Lineas", ct);

    /// <summary>GET Informe/{num}/Informe — cabecera con su TipoInforme (define columnas visibles).</summary>
    public Task<ApiRespuesta<Informe>> GetInformeAsync(int numInforme, CancellationToken ct = default)
        => _api.GetAsync<Informe>($"Informe/{numInforme}/Informe", ct);
}
