using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Horarios de personal contra WebApiRW (endpoint aditivo api/Horarios). Fase visualización.</summary>
public sealed class HorariosService
{
    private readonly ApiWeb _api;

    public HorariosService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Horarios/Secciones — secciones con horario.</summary>
    public Task<ApiRespuesta<List<HorarioSeccion>>> GetSeccionesAsync(CancellationToken ct = default)
        => _api.GetAsync<List<HorarioSeccion>>("Horarios/Secciones", ct);

    /// <summary>GET Horarios/Semana/{alm}/{seccion}?historico= — cuadro semanal en texto.</summary>
    public Task<ApiRespuesta<HorarioSemana>> GetSemanaAsync(short codAlm, byte seccion, bool historico, CancellationToken ct = default)
        => _api.GetAsync<HorarioSemana>($"Horarios/Semana/{codAlm}/{seccion}?historico={historico}", ct);

    /// <summary>GET Report/Horarios|HorarioHistorico — PDF del cuadro (mismos rdlc de R3).</summary>
    public Task<ApiRespuesta<byte[]>> GetPdfAsync(short codAlm, byte seccion, bool historico, CancellationToken ct = default)
        => _api.GetBytesAsync(historico
            ? $"Report/HorarioHistorico/{codAlm}/{seccion}"
            : $"Report/Horarios/{codAlm}/{seccion}", ct);
}
