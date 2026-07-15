namespace R5.Core.Entidades;

/// <summary>Pasillo del recorrido de preparación (canon R4): permite saltar el picking
/// al primer producto de un pasillo concreto.</summary>
public sealed class PrepPedCentralPasillo
{
    public string Pasillo { get; set; } = "";
    public int CountProdPedidos { get; set; }
    public int CountProdPreparados { get; set; }
}

/// <summary>Alta de producto NO incluido en el pedido durante la preparación (canon R4).</summary>
public sealed class PrepPedCentralLinInsertDTO
{
    public int CodProd { get; set; }
    public int CodUsuario { get; set; }
    public short CodAlm { get; set; }
    public short CodAlmCentral { get; set; }
    public decimal CantPedida { get; set; }
    public DateTime Fecha { get; set; }
    public int NumPedido { get; set; }
}

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
