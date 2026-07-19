namespace R5.Core.Entidades;

/// <summary>
/// Usuario de RW. Propiedades = contrato de WebApiRW (canon R4, GET api/Usuario/{cod}).
/// POCO puro: las llamadas viven en Servicios/UsuarioService, no en la entidad.
/// </summary>
public sealed class Usuario
{
    public short CodUsuario { get; set; }
    public string? DescUsuario { get; set; } = "";
    public bool PermiteCambioAlm { get; set; }
    public short NumTerminal { get; set; }
    public bool TraspasosPorCajas { get; set; }
    public bool FechaDocHoy { get; set; }
    public bool SolicitarUserPwd { get; set; }
    public bool MostrarEanPreparacion { get; set; }
    public bool CodProdEnConsultas { get; set; }
    public bool CodProvEnCompras { get; set; }
    public bool AsocEanCaja { get; set; }
    public string? RedIpUsuario { get; set; }
    public Almacen Almacen { get; set; } = new();
    public List<Menu> Menus { get; set; } = new();
    public bool PermisoUbi { get; set; }
    public bool FinalizarPedCentral { get; set; }
    public string? Mail { get; set; }
    /// <summary>Permite cambiar el almacén destino en ajustes (junto con el flag del almacén).</summary>
    public bool CambAlmDestAjustes { get; set; }
    /// <summary>Permite anular facturas TPV (módulo 28).</summary>
    public bool FactTpvAnula { get; set; }
    /// <summary>Perfil sobre la ficha de cliente TPV: 0 no crear, 1 modif. parcial, 2 modif. todo, 3 avanzadas.</summary>
    public byte PerfilClienteTpv { get; set; }
    /// <summary>Empleado ligado a la sesión (0 = usuario Pda puro). La sesión SIEMPRE es RW_Usuario.</summary>
    public int Empleado { get; set; }
}
