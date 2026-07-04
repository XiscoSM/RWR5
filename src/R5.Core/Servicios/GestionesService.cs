using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Gestiones de empresa (firmas por empleado) contra WebApiRW (endpoint aditivo api/Gestiones).</summary>
public sealed class GestionesService
{
    private readonly ApiWeb _api;

    public GestionesService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Gestiones/Empleado/{num} — valida el empleado y trae nombre + saldo de vacaciones.</summary>
    public Task<ApiRespuesta<EmpleadoInfo>> GetEmpleadoAsync(int empleado, CancellationToken ct = default)
        => _api.GetAsync<EmpleadoInfo>($"Gestiones/Empleado/{empleado}", ct);

    /// <summary>GET Gestiones/Pendientes/{empleado} — gestiones pendientes de firma.</summary>
    public Task<ApiRespuesta<List<GestionEmpresaUser>>> GetPendientesAsync(int empleado, CancellationToken ct = default)
        => _api.GetAsync<List<GestionEmpresaUser>>($"Gestiones/Pendientes/{empleado}", ct);

    /// <summary>GET Gestiones/Firmadas/{empleado} — firmadas por el empleado.</summary>
    public Task<ApiRespuesta<List<GestionEmpresaUser>>> GetFirmadasAsync(int empleado, CancellationToken ct = default)
        => _api.GetAsync<List<GestionEmpresaUser>>($"Gestiones/Firmadas/{empleado}", ct);

    /// <summary>GET Gestiones/FirmadasResponsable/{empleado} — firmadas como responsable.</summary>
    public Task<ApiRespuesta<List<GestionEmpresaUser>>> GetFirmadasResponsableAsync(int empleado, CancellationToken ct = default)
        => _api.GetAsync<List<GestionEmpresaUser>>($"Gestiones/FirmadasResponsable/{empleado}", ct);

    /// <summary>POST Gestiones/{empleado}/Firmar/{mov} — firma la gestión (el proc valida el nº).</summary>
    public Task<ApiRespuesta> FirmarAsync(int empleado, int mov, CancellationToken ct = default)
        => _api.PostAsync($"Gestiones/{empleado}/Firmar/{mov}", cuerpo: null, ct);

    /// <summary>GET Gestiones/Catalogo/{tipo}?usuario= — gestiones solicitables (vacío = sin permiso).</summary>
    public Task<ApiRespuesta<List<GestionEmpresa>>> GetCatalogoAsync(byte tipoGestion, int empleado, CancellationToken ct = default)
        => _api.GetAsync<List<GestionEmpresa>>($"Gestiones/Catalogo/{tipoGestion}?usuario={empleado}", ct);

    /// <summary>POST Gestiones/Solicitud — alta de solicitud (EPI, vacaciones, Madisa…).</summary>
    public Task<ApiRespuesta> SolicitarAsync(GestionSolicitudInsertDTO solicitud, CancellationToken ct = default)
        => _api.PostAsync("Gestiones/Solicitud", solicitud, ct);
}
