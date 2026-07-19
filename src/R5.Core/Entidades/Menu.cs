namespace R5.Core.Entidades;

/// <summary>
/// Opción de menú que la API devuelve para el usuario (permisos por BD).
/// El mapeo CodMenu → ruta/icono de R5 vive en Servicios/CatalogoModulos.
/// </summary>
public sealed class Menu
{
    public byte CodMenu { get; set; }
    public string DescMenu { get; set; } = "";
    public bool SolicitarPwd { get; set; }
    /// <summary>Minutos de inactividad hasta bloquear la sesión en este menú (0 = no bloquea).</summary>
    public short MinutosBloqueo { get; set; }

    public Menu() { }

    public Menu(byte codMenu, string descMenu, bool solicitarPwd)
    {
        CodMenu = codMenu;
        DescMenu = descMenu;
        SolicitarPwd = solicitarPwd;
    }
}
