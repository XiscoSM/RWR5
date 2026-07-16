namespace R5.Core.Entidades;

/// <summary>Albarán de proveedor (cabecera, canon R4).</summary>
public sealed class Albaran
{
    public DateTime Fecha { get; set; }
    public int NumAlbaran { get; set; }
    public Almacen Alm { get; set; } = new();
    public Proveedor Prov { get; set; } = new();
    public int NumPedido { get; set; }
    public bool Reg { get; set; }
    public bool Fact { get; set; }
    public int Propietario { get; set; }
    public long LinkPdf { get; set; }
    public string NumAlbProv { get; set; } = "";
    public string Comentario { get; set; } = "";
    public long Prods { get; set; }
    public decimal CantTotal { get; set; }
    public decimal ImpNetoComercial { get; set; }
    public decimal ImpNetoFact { get; set; }
    public decimal ImpTotalFact { get; set; }
}

/// <summary>Línea de albarán (canon R4).</summary>
public sealed class AlbaranLin
{
    public int Linea { get; set; }
    public Producto Prod { get; set; } = new();
    public short Cajas { get; set; }
    public decimal Cant { get; set; }
    public bool PrecioAbierto { get; set; }
    public decimal CantAnt { get; set; }
    public double PTarifa { get; set; }
    public double PNC { get; set; }
    public double DtosComerciales { get; set; }
    public string? DtosComercialesTexto { get; set; } = "";
    public Ean Ean { get; set; } = new();
    public byte TipoPrecio { get; set; }
    public byte TipoLinAtipico { get; set; }
    public bool Coment { get; set; }
    public short CajasPedida { get; set; }
    public short UnidsCaja { get; set; }
    public decimal CantPedida { get; set; }
    public decimal CantRecibida { get; set; }
    public decimal CantAlbTotal { get; set; }
    public decimal Stock { get; set; }
    public decimal PncFecha { get; set; }
    public string? Datos { get; set; } = "";

    // Perfil de trazabilidad de compra del producto (Compra_* de R3):
    // 0 = no se pide, 1 = se pide, 2 = se pide y retiene el valor para el siguiente.
    public byte CompraFechaCad { get; set; }
    public bool CompraFechaCadAuto { get; set; }
    public bool CompraLoteProv { get; set; }
    public byte CompraTemperatura { get; set; }
    public byte CompraEstiba { get; set; }
    public byte CompraFechaNac { get; set; }
    public byte CompraFechaSac { get; set; }
    public byte CompraPaisNac { get; set; }
    public byte CompraPaisEng { get; set; }
    public byte CompraPaisSac { get; set; }
    public byte CompraCanal { get; set; }
    public byte CompraCrotal { get; set; }
    public byte CompraCategoria { get; set; }
    public byte CompraRegSanitarioDesp { get; set; }
    public byte CompraRegSanitarioSac { get; set; }
}

/// <summary>Datos de trazabilidad de una línea de recepción (ProductoDatos de R3):
/// construye el mismo JSON que DatosToJson consume el proc (openjson server-side).</summary>
public sealed class DatosTrazabilidad
{
    public string LoteProv = "";
    public int Canal;
    public string Categoria = "";
    public string Crotal = "";
    public bool EstibaOk;
    public bool TempOk;
    public DateTime? FechaCad;
    public DateTime? FechaNac;
    public DateTime? FechaSac;
    public string PaisNac = "";
    public string PaisEng = "";
    public string PaisSac = "";
    public string RegSanitarioDesp = "";
    public string RegSanitarioSac = "";

    /// <summary>Mismo contrato que ProductoDatos.DatosToJson de R3 (Entidades.vb:5409):
    /// solo emite los campos cuyo flag del perfil está activo; fechas dd/MM/yyyy;
    /// booleanos como 0/1. El lote va además como parámetro aparte del proc.</summary>
    public string ToJson(AlbaranLin lin)
    {
        var t = new List<string>();
        if (lin.CompraLoteProv || lin.Prod.ControlaLotes) t.Add($"\"NumLoteProv\":\"{Escapa(LoteProv)}\"");
        if (lin.CompraCanal > 0) t.Add($"\"Canal\":{Canal}");
        if (lin.CompraCategoria > 0) t.Add($"\"Categoria\":\"{Escapa(Categoria)}\"");
        if (lin.CompraCrotal > 0) t.Add($"\"Crotal\":\"{Escapa(Crotal)}\"");
        if (lin.CompraEstiba > 0) t.Add($"\"EstibaOk\":{(EstibaOk ? 1 : 0)}");
        if ((lin.CompraFechaCad > 0 || lin.Prod.DiasCaducidad > 0) && FechaCad is not null)
            t.Add($"\"FechaCad\":\"{FechaCad:dd/MM/yyyy}\"");
        if (lin.CompraFechaNac > 0 && FechaNac is not null) t.Add($"\"FechaNac\":\"{FechaNac:dd/MM/yyyy}\"");
        if (lin.CompraFechaSac > 0 && FechaSac is not null) t.Add($"\"FechaSac\":\"{FechaSac:dd/MM/yyyy}\"");
        if (lin.CompraPaisEng > 0) t.Add($"\"PaisEng\":\"{Escapa(PaisEng)}\"");
        if (lin.CompraPaisNac > 0) t.Add($"\"PaisNac\":\"{Escapa(PaisNac)}\"");
        if (lin.CompraPaisSac > 0) t.Add($"\"PaisSac\":\"{Escapa(PaisSac)}\"");
        if (lin.CompraRegSanitarioDesp > 0) t.Add($"\"RegSanitarioDesp\":\"{Escapa(RegSanitarioDesp)}\"");
        if (lin.CompraRegSanitarioSac > 0) t.Add($"\"RegSanitarioSac\":\"{Escapa(RegSanitarioSac)}\"");
        if (lin.CompraTemperatura > 0) t.Add($"\"TempOk\":{(TempOk ? 1 : 0)}");
        return t.Count == 0 ? "" : "{" + string.Join(",", t) + "}";
    }

    private static string Escapa(string s) => s.Replace("\\", "\\\\").Replace("\"", "\\\"");
}
