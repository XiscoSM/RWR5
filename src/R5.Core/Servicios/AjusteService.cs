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

    /// <summary>GET Ajuste/ListTipoAjustes — tipos de ajuste (merma, rotura, consumo propio…).</summary>
    public Task<ApiRespuesta<List<TipoAjuste>>> GetTiposAsync(CancellationToken ct = default)
        => _api.GetAsync<List<TipoAjuste>>("Ajuste/ListTipoAjustes", ct);

    /// <summary>GET Ajuste/ListProgramas — programas de gestión (destino contable del ajuste).</summary>
    public Task<ApiRespuesta<List<Programa>>> GetProgramasAsync(CancellationToken ct = default)
        => _api.GetAsync<List<Programa>>("Ajuste/ListProgramas", ct);

    /// <summary>GET Ajuste/{num}/{fecha}/Linea — producto a ajustar, con su coste y lo ya ajustado.</summary>
    public Task<ApiRespuesta<AjusteLin>> GetLineaProductoAsync(int numAjuste, DateTime fecha, short codAlmOrigen, short codAlmDestino, int codProd, long codEan, CancellationToken ct = default)
        => _api.GetAsync<AjusteLin>(
            $"Ajuste/{numAjuste}/{fecha:yyyy-MM-dd}/Linea?codEan={codEan}&codProd={codProd}&lote=0&codAlmDestino={codAlmDestino}&codAlmOrigen={codAlmOrigen}", ct);

    /// <summary>POST Ajuste/{num}/{fecha}/{prod} — añade línea (num 0 crea el ajuste; devuelve su número).</summary>
    public Task<ApiRespuesta<Documento>> PostLineaAsync(AjusteLinInsertDTO linea, CancellationToken ct = default)
        => _api.PostAsync<Documento>($"Ajuste/{linea.NumAjuste}/{linea.Fecha:yyyy-MM-dd}/{linea.CodProd}", linea, ct);

    /// <summary>POST Ajuste/{num}/{fecha}/Registrar/{codUsuario} — registra el ajuste.</summary>
    public Task<ApiRespuesta> RegistrarAsync(int numAjuste, DateTime fecha, short codUsuario, CancellationToken ct = default)
        => _api.PostAsync($"Ajuste/{numAjuste}/{fecha:yyyy-MM-dd}/Registrar/{codUsuario}", cuerpo: null, ct);
}
