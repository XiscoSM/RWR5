namespace R5.Core.Entidades;

/// <summary>Producto elaborable del almacén (módulo 32).</summary>
public sealed class ElabProducto
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public bool ControlPuntCrit { get; set; }
}

/// <summary>Elaboración de la lista del usuario (abiertas o cerradas).</summary>
public sealed class ElabResumen
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public DateTime Fecha { get; set; }
    public int Elaboracion { get; set; }
    public int Lote { get; set; }
    public bool Reg { get; set; }
}

/// <summary>Ingrediente de la receta, con lo ya acumulado (CantTotal).</summary>
public sealed class ElabIngrediente
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public bool Obligatorio { get; set; }
    public decimal CantTotal { get; set; }
}

/// <summary>Captura de ingrediente ya introducida en la elaboración.</summary>
public sealed class ElabIngredienteIntro
{
    public int Prod { get; set; }
    public string DescProd { get; set; } = "";
    public decimal Cant { get; set; }
}

/// <summary>Punto crítico (APPCC) con su rango. LineaCalc>0: rango relativo a otro punto.</summary>
public sealed class ElabPtoCritico
{
    public int LineaPtoCritico { get; set; }
    public string Descripcion { get; set; } = "";
    public string DescTipoValor { get; set; } = "";
    public decimal ValorIntro { get; set; }
    public decimal ValorIni { get; set; }
    public decimal ValorFin { get; set; }
    public int LineaCalc { get; set; }
}

/// <summary>Alta de ingrediente: el EAN128 identifica el lote; elaboración 0 crea una nueva.</summary>
public sealed class ElabIngredienteInsertDTO
{
    public int ProdElab { get; set; }
    public DateTime Fecha { get; set; }
    public int Elaboracion { get; set; }
    public string Ean128 { get; set; } = "";
    public decimal Cant { get; set; }
    public short CodAlm { get; set; }
    public short CodUsuario { get; set; }
}

/// <summary>Resultado del alta: fecha/número reales de la elaboración (por si se creó nueva).</summary>
public sealed class ElabIngredienteResultado
{
    public DateTime Fecha { get; set; }
    public int Elaboracion { get; set; }
    public int IngredientePadre { get; set; }
    public decimal CantTotal { get; set; }
}
