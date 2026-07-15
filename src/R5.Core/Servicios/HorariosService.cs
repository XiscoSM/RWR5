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

    /// <summary>GET Horarios/Cuadrante/{alm} — cuadrante completo con el JSON crudo por celda (editor).</summary>
    public Task<ApiRespuesta<List<HorarioCuadranteFila>>> GetCuadranteAsync(short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<HorarioCuadranteFila>>($"Horarios/Cuadrante/{codAlm}", ct);

    /// <summary>GET Horarios/Calendario/{alm} — festivos del calendario laboral (yyyyMMdd).</summary>
    public Task<ApiRespuesta<List<int>>> GetCalendarioAsync(short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<int>>($"Horarios/Calendario/{codAlm}", ct);

    /// <summary>POST Horarios/Celda — graba una celda del cuadrante (el proc valida).</summary>
    public Task<ApiRespuesta> PostCeldaAsync(HorarioCeldaUpdate celda, CancellationToken ct = default)
        => _api.PostAsync("Horarios/Celda", celda, ct);

    /// <summary>POST Horarios/Celda/Comentario — comentario ligado a un marcaje ya registrado.</summary>
    public Task<ApiRespuesta> PostComentarioAsync(int usuario, DateTime fecha, string comentario, CancellationToken ct = default)
        => _api.PostAsync($"Horarios/Celda/Comentario?usuario={usuario}&fecha={fecha:yyyy-MM-dd}", comentario, ct);

    /// <summary>POST Horarios/Empleado/{usuario}/Seccion/{seccion} — cambia la sección del empleado.</summary>
    public Task<ApiRespuesta> PostSeccionEmpleadoAsync(int usuario, byte seccion, CancellationToken ct = default)
        => _api.PostAsync($"Horarios/Empleado/{usuario}/Seccion/{seccion}", cuerpo: null, ct);

    /// <summary>GET Report/Horarios|HorarioHistorico — PDF del cuadro (mismos rdlc de R3).
    /// fechaIni/fechaFin acotan las semanas impresas (el endpoint las acepta; sin ellas
    /// imprime toda la sección).</summary>
    public Task<ApiRespuesta<byte[]>> GetPdfAsync(short codAlm, byte seccion, bool historico, DateTime? fechaIni = null, DateTime? fechaFin = null, CancellationToken ct = default)
    {
        string rango = fechaIni is null ? "" : $"?fechaIni={fechaIni:yyyy-MM-dd}&fechaFin={(fechaFin ?? fechaIni):yyyy-MM-dd}";
        return _api.GetBytesAsync(historico
            ? $"Report/HorarioHistorico/{codAlm}/{seccion}{rango}"
            : $"Report/Horarios/{codAlm}/{seccion}{rango}", ct);
    }
}
