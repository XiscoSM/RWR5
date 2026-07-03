using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Login y datos de usuario contra WebApiRW (rutas del canon R4).</summary>
public sealed class UsuarioService
{
    private readonly ApiWeb _api;

    public UsuarioService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Usuario/{cod}?pwd= — valida credenciales y devuelve el usuario con menús y almacén.</summary>
    public Task<ApiRespuesta<Usuario>> LoginAsync(short codUsuario, int pwd, CancellationToken ct = default)
        => _api.GetAsync<Usuario>($"Usuario/{codUsuario}?pwd={pwd}", ct);

    /// <summary>GET Usuario/{cod}/Almacen/{codAlm} — almacén si el usuario tiene permiso.</summary>
    public Task<ApiRespuesta<Almacen>> GetAlmacenAsync(short codUsuario, short codAlm, CancellationToken ct = default)
        => _api.GetAsync<Almacen>($"Usuario/{codUsuario}/Almacen/{codAlm}", ct);
}
