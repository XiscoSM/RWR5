using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Ajustes de stock contra WebApiRW (canon R4). Fase actual: consulta.</summary>
public sealed class AjusteService
{
    private readonly ApiWeb _api;

    public AjusteService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Ajuste/List/{codAlm}?codUsuario=&amp;registrado=&amp;top= — ajustes del usuario.</summary>
    public Task<ApiRespuesta<List<Ajuste>>> GetAjustesAsync(short codAlm, short codUsuario, bool registrado, short top = 50, CancellationToken ct = default)
        => _api.GetAsync<List<Ajuste>>($"Ajuste/List/{codAlm}?codUsuario={codUsuario}&registrado={registrado}&top={top}", ct);

    /// <summary>GET Ajuste/{num}/{fecha}/Lineas/Agrup — líneas agrupadas del ajuste.</summary>
    public Task<ApiRespuesta<List<AjusteLin>>> GetLineasAsync(int numAjuste, DateTime fecha, short top = 200, CancellationToken ct = default)
        => _api.GetAsync<List<AjusteLin>>($"Ajuste/{numAjuste}/{fecha:yyyy-MM-dd}/Lineas/Agrup?top={top}", ct);
}
