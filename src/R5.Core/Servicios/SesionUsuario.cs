using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>
/// Estado de sesión de la app: usuario autenticado y su almacén activo.
/// Singleton; las páginas se suscriben a Cambio para refrescarse.
/// </summary>
public sealed class SesionUsuario
{
    public Usuario? Usuario { get; private set; }

    public bool Autenticado => Usuario is not null && Usuario.CodUsuario > 0;

    public event Action? Cambio;

    public void IniciarSesion(Usuario usuario)
    {
        Usuario = usuario;
        Cambio?.Invoke();
    }

    public void CambiarAlmacen(Almacen almacen)
    {
        if (Usuario is null) return;
        Usuario.Almacen = almacen;
        Cambio?.Invoke();
    }

    public void CerrarSesion()
    {
        Usuario = null;
        Cambio?.Invoke();
    }
}
