namespace R5.Core.Entidades;

/// <summary>Alta de línea de informe manual (DTO del canon R4, POST Informe/{num}/{fecha}/{prod}).</summary>
public sealed class InformeLinInsertDTO
{
    public short CodUsuario { get; set; }
    public byte Tipo { get; set; }
    public int NumInforme { get; set; }
    public DateTime Fecha { get; set; }
    public short CodAlmOrigen { get; set; }
    public short CodAlmDest { get; set; }
    public int CodProd { get; set; }
    public short Cajas { get; set; }
    public decimal Cant { get; set; }
    public decimal PrecioA { get; set; }
    public decimal PrecioB { get; set; }
    public decimal PrecioC { get; set; }
    public string ValorA { get; set; } = "";
    public string ValorB { get; set; } = "";
    public string ValorC { get; set; } = "";
    public long CodEan { get; set; }
}
