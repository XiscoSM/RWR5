namespace R5.Core.Entidades;

/// <summary>Fila de la cola de etiquetas EanList (X_EtiquetasR2, port del Delphi Fex Etiqueta).</summary>
public sealed class EanListPendiente
{
    public int NLinea { get; set; }
    public int CodProducto { get; set; }
    public string Ean { get; set; } = "";
    public string Descripcio { get; set; } = "";
    public decimal Pvp { get; set; }
    public string Oferta { get; set; } = "";
    public byte TipoPrint { get; set; }
    public string Equipo { get; set; } = "";
    public DateTime? Fecha { get; set; }

    /// <summary>Nombre humano del formato de impresión (mismos códigos que R3/Delphi).</summary>
    public string DescFormato => TipoPrint switch
    {
        0 => "Estándar",
        1 => "Perecederos",
        2 => "Fruta",
        3 => "Oferta",
        4 => "Oferta (papel especial)",
        5 => "Folio/textil",
        _ => $"Tipo {TipoPrint}"
    };
}

/// <summary>Fila de etiquetas de un albarán (PROC_EanList_MetoAlb).</summary>
public sealed class EanListMetoAlbLinea
{
    public int Prod { get; set; }
    public string Ean { get; set; } = "";
    public string DescProdCompleta { get; set; } = "";
    public decimal Cant { get; set; }
    public decimal Pvp { get; set; }
}
