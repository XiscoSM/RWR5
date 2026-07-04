using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Etiquetas Madisa (estantería) contra WebApiRW (canon R4). Los procs son por tienda.</summary>
public sealed class EtiquetaService
{
    private readonly ApiWeb _api;

    public EtiquetaService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>POST Madisa/Etiqueta/{alm}/Ean — consulta y/o solicita la etiqueta según Option.</summary>
    public Task<ApiRespuesta<Producto>> PostEtiquetaAsync(EanUserTermDTO solicitud, CancellationToken ct = default)
        => _api.PostAsync<Producto>($"Madisa/Etiqueta/{solicitud.CodAlm}/Ean", solicitud, ct);

    /// <summary>GET Madisa/Etiqueta/{alm}/{terminal} — cola de etiquetas PDA del terminal.</summary>
    public Task<ApiRespuesta<List<Producto>>> GetColaAsync(short codAlm, short numTerminal, CancellationToken ct = default)
        => _api.GetAsync<List<Producto>>($"Madisa/Etiqueta/{codAlm}/{numTerminal}", ct);

    /// <summary>DELETE Madisa/Etiqueta/{alm}/{terminal} — vacía la cola del terminal.</summary>
    public Task<ApiRespuesta> VaciarColaAsync(short codAlm, short numTerminal, CancellationToken ct = default)
        => _api.DeleteAsync($"Madisa/Etiqueta/{codAlm}/{numTerminal}", ct);
}
