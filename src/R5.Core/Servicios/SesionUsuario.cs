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

    // Menús con SolicitarPwd ya validados en esta sesión (no se re-pide al volver a entrar).
    private readonly HashSet<byte> _menusValidados = new();

    public bool MenuValidado(byte codMenu) => _menusValidados.Contains(codMenu);

    public void ValidarMenu(byte codMenu) => _menusValidados.Add(codMenu);

    public void IniciarSesion(Usuario usuario)
    {
        Usuario = usuario;
        _menusValidados.Clear();
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
        _menusValidados.Clear();
        Cambio?.Invoke();
    }
}
