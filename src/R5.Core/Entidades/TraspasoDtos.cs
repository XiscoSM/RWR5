namespace R5.Core.Entidades;

/// <summary>Alta de línea de traspaso (DTO del canon R4, POST Traspasos/{num}/{fecha}/{prod}).</summary>
public sealed class TraspLinInsertDTO
{
    public DateTime Fecha { get; set; }
    public int NumTraspaso { get; set; }
    public short CodAlmOrigen { get; set; }
    public short CodAlmDest { get; set; }
    public int CodProd { get; set; }
    public long CodEan { get; set; }
    public short Cajas { get; set; }
    public short UnidCaja { get; set; }
    public decimal Cant { get; set; }
    public decimal Importe { get; set; }
    public short CodUsuario { get; set; }
    public int Lote { get; set; }
    public bool Registrado { get; set; }
    public DateTime FechaPedido { get; set; }
    public int NumPedido { get; set; }
}

/// <summary>Respuesta del alta de línea de traspaso (canon R4): número/línea asignados.</summary>
public sealed class TraspLinSelectDTO
{
    public DateTime Fecha { get; set; }
    public int NumTraspaso { get; set; }
    public short CodAlmOrigen { get; set; }
    public short CodAlmDest { get; set; }
    public int CodProd { get; set; }
    public long CodEan { get; set; }
    public int Lote { get; set; }
    public int Linea { get; set; }
    public DateTime FechaPedido { get; set; }
    public int NumPedido { get; set; }
}
