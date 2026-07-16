namespace R5.Core.Entidades;

/// <summary>Traspaso entre almacenes (cabecera, canon R4).</summary>
public sealed class Traspaso
{
    public DateTime Fecha { get; set; }
    public int NumTraspaso { get; set; }
    public Almacen? AlmOrigen { get; set; }
    public Almacen? AlmDest { get; set; }
    public bool Registrado { get; set; }
    public decimal Importe { get; set; }
    public long CountProds { get; set; }
}

/// <summary>Línea de traspaso (canon R4).</summary>
public sealed class TraspasoLin
{
    public int Linea { get; set; }
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public short Cajas { get; set; }
    public decimal Cant { get; set; }
    public decimal Importe { get; set; }
    public short CajasEnv { get; set; }
    public decimal CantEnv { get; set; }
    /// <summary>Cantidad pedida en el pedido central vinculado (0 sin vínculo).</summary>
    public decimal CantPedida { get; set; }
}
