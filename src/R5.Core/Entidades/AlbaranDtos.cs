namespace R5.Core.Entidades;

/// <summary>Documento creado/afectado por un insert (canon R4): número y línea asignados.</summary>
public sealed class Documento
{
    public int NumDoc { get; set; }
    public int LineaDoc { get; set; }
}

/// <summary>Alta de línea de albarán de recepción (DTO del canon R4, POST Albaran/{alm}/{fecha}/{prod}).</summary>
public sealed class AlbLinInsertDTO
{
    public DateTime Fecha { get; set; }
    public int CodProv { get; set; }
    public short CodAlm { get; set; }
    public decimal Cant { get; set; }
    public byte TipoLinAtipico { get; set; }
    public short Cajas { get; set; }
    public decimal PTarifa { get; set; }
    public string NumLoteProv { get; set; } = "";
    public int NumPedido { get; set; }
    public string DtosComercialesTexto { get; set; } = "";
    public decimal Iva { get; set; }
    public bool Reg { get; set; }
    public long CodEan { get; set; }
    public string Datos { get; set; } = "";
    public int NumAlbaran { get; set; }
    public int CodGama { get; set; }
    public int CodProd { get; set; }
    public decimal PrecioNetoComercial { get; set; }
    public short CodUsuario { get; set; }
    public short UnidsCaja { get; set; }
    public decimal DtosComerciales { get; set; }
    public string NumAlbProv { get; set; } = "";
    public DateTime? FechaCaducidad { get; set; }
    public int Linea { get; set; }
    public byte TipoPrecio { get; set; }
    public decimal PncFecha { get; set; }
}
