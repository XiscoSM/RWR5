namespace R5.Core.Entidades;

/// <summary>Alta de línea de ajuste de stock (DTO del canon R4, POST Ajuste/{num}/{fecha}/{prod}).</summary>
public sealed class AjusteLinInsertDTO
{
    public DateTime Fecha { get; set; }
    public int NumAjuste { get; set; }
    public byte Tipo { get; set; }
    public short CodAlmOrigen { get; set; }
    public short CodAlmDest { get; set; }
    public short ProgDest { get; set; }
    public short CodUsuario { get; set; }
    public int CodProd { get; set; }
    public decimal Cant { get; set; }
    public decimal ImporteCoste { get; set; }
    public int Lote { get; set; }
    public long CodEan { get; set; }
    public bool Registrado { get; set; }
    public int Linea { get; set; }
}
