namespace R5.Core.Entidades;

/// <summary>Alta de conteo de inventario (DTO del canon R4, POST Inventarios/{alm}/{fecha}/{prod}).</summary>
public sealed class InvLinInsertDTO
{
    public DateTime Fecha { get; set; }
    public short CodAlm { get; set; }
    public int CodProd { get; set; }
    public long CodEan { get; set; }
    public decimal Cant { get; set; }
    public short CodUsuario { get; set; }
    public short NumTerminal { get; set; }
}
