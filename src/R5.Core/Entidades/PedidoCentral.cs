namespace R5.Core.Entidades;

/// <summary>
/// Pedido a central (cabecera). Propiedades = entidad de WebApiRW (canon R4);
/// DescEstado se calcula en cliente como en RWR4.
/// </summary>
public sealed class PedidoCentral
{
    public DateTime Fecha { get; set; }
    public int NumPedido { get; set; }
    public Almacen? Alm { get; set; }
    public Almacen? AlmCentral { get; set; }
    public short CodUsuario { get; set; }
    public byte Estado { get; set; }
    public CentroDistribucion? CentroDist { get; set; }

    public string DescEstado => Estado switch
    {
        0 => "Abierto",
        1 => "Cerrado",
        2 => "En preparación",
        3 => "Preparado",
        _ => "No definido"
    };
}
