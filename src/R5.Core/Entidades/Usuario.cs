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
}
