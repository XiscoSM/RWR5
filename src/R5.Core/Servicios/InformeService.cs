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

    /// <summary>GET Informe/ListTipoInformes — tipos de informe manual con su configuración de campos.</summary>
    public Task<ApiRespuesta<List<TipoInforme>>> GetTiposAsync(CancellationToken ct = default)
        => _api.GetAsync<List<TipoInforme>>("Informe/ListTipoInformes", ct);

    /// <summary>GET Informe/Tipo/{tipo}/Prod?codEan= — producto para el informe (RellenarCampos precarga precios/valores).</summary>
    public Task<ApiRespuesta<InformeLin>> GetProdPorEanAsync(short tipo, short codAlm, long codEan, CancellationToken ct = default)
        => _api.GetAsync<InformeLin>($"Informe/Tipo/{tipo}/Prod?codAlm={codAlm}&codEan={codEan}", ct);

    /// <summary>GET Informe/Tipo/{tipo}/Prod/{codProd} — producto por código.</summary>
    public Task<ApiRespuesta<InformeLin>> GetProdPorCodAsync(short tipo, short codAlm, int codProd, CancellationToken ct = default)
        => _api.GetAsync<InformeLin>($"Informe/Tipo/{tipo}/Prod/{codProd}?codAlm={codAlm}", ct);

    /// <summary>POST Informe/{num}/{fecha}/{prod} — añade línea (num 0 crea el informe; devuelve su número).</summary>
    public Task<ApiRespuesta<Documento>> PostLineaAsync(InformeLinInsertDTO linea, CancellationToken ct = default)
        => _api.PostAsync<Documento>($"Informe/{linea.NumInforme}/{linea.Fecha:yyyy-MM-dd}/{linea.CodProd}", linea, ct);

    /// <summary>POST Informe/{num}/Registrar — registra el informe.</summary>
    public Task<ApiRespuesta> RegistrarAsync(int numInforme, CancellationToken ct = default)
        => _api.PostAsync($"Informe/{numInforme}/Registrar", cuerpo: null, ct);
}
