namespace R5.Core.Entidades;

/// <summary>Preparación de pedidos de central (cabecera, canon R4).</summary>
public sealed class PrepPedCentral
{
    public int Pedido { get; set; }
    public DateTime Fecha { get; set; }
    public Almacen Alm { get; set; } = new();
    public int CountPendientes { get; set; }
    public int CountNoReg { get; set; }
    public int CountPedidos { get; set; }
    public DateTime FechaCierre { get; set; }
    public DateTime FechaPreparar { get; set; }
    public bool Todos { get; set; }
    public int Preparados { get; set; }
}

/// <summary>Línea de preparación (canon R4).</summary>
public sealed class PrepPedCentralLin
{
    public Producto Prod { get; set; } = new();
    public Ean Ean { get; set; } = new();
    public short CajasEnviadas { get; set; }
    public short CajasPedidas { get; set; }
    public decimal CantEnviada { get; set; }
    public decimal CantPedida { get; set; }
    public decimal StockCentral { get; set; }
}
