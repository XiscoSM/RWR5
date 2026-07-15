namespace R5.Core.Entidades;

/// <summary>Sección con horario (canon R4, api/Horarios/Secciones).</summary>
public sealed class HorarioSeccion
{
    public byte Seccion { get; set; }
    public string DescSeccion { get; set; } = "";
}

/// <summary>Fila del cuadro semanal: empleado + celdas ya en texto.</summary>
public sealed class HorarioSemanaFila
{
    public string Nombre { get; set; } = "";
    public List<string> Celdas { get; set; } = new();
}

/// <summary>Cuadro semanal de una sección (columnas dinámicas del proc).</summary>
public sealed class HorarioSemana
{
    public List<string> Columnas { get; set; } = new();
    public List<HorarioSemanaFila> Filas { get; set; } = new();
}

/// <summary>Fila del cuadrante del editor (api/Horarios/Cuadrante): separador de
/// semana si Usuario=0; las 7 celdas llegan con el JSON crudo de HorarioCelda.</summary>
public sealed class HorarioCuadranteFila
{
    public byte Seccion { get; set; }
    public int Usuario { get; set; }
    public int AnyoSemana { get; set; }
    public string Nombre { get; set; } = "";
    public string Saldo { get; set; } = "";
    public List<string?> Celdas { get; set; } = new();
    public string? Resumen { get; set; }
}

/// <summary>Celda a grabar en el cuadrante (POST api/Horarios/Celda).</summary>
public sealed class HorarioCeldaUpdate
{
    public int Usuario { get; set; }
    public DateTime Fecha { get; set; }
    public byte TipoDiaTrab { get; set; }
    public string HorarioJson { get; set; } = "";
    public short Minutos { get; set; }
}

/// <summary>Celda del cuadrante, port de HorarioCelda de R3: H = horario en minutos
/// (pares inicio-fin), P = advertencias, T = tipo de día (0 trabajo, 1 vacaciones,
/// 2 comp. festivo, 3 horas extras, 4 otros, 99 baja, 100 libre), C = comentario.
/// Los nombres de una letra son el contrato JSON de la BD — no renombrar.</summary>
public sealed class HorarioCelda
{
    public short[] H { get; set; } = Array.Empty<short>();
    public byte[] P { get; set; } = Array.Empty<byte>();
    public byte T { get; set; }
    public string? C { get; set; }

    private static readonly string[] _tipoInc =
        { "+ 2 Dom.Fest.", "6 días libres Mes", "Desc. entre Días", "2 días consecutivos libres" };

    /// <summary>Celda libre (T=100), el valor de una casilla vacía en R3.</summary>
    public static HorarioCelda Libre() => new() { T = 100 };

    public static HorarioCelda? FromJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<HorarioCelda>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>JSON con el mismo contrato que ExportToJson de R3: los días con tipo
    /// exportan {"T":n}; los de trabajo {"H":[...]}; el comentario se añade si existe.</summary>
    public string ToJson()
    {
        var partes = new List<string>();
        if (T > 0) partes.Add($"\"T\":{T}");
        else if (H.Length > 0) partes.Add($"\"H\":[{string.Join(",", H)}]");
        if (!string.IsNullOrEmpty(C))
            partes.Add($"\"C\":{System.Text.Json.JsonSerializer.Serialize(C)}");
        return "{" + string.Join(",", partes) + "}";
    }

    /// <summary>Minutos trabajados: suma de (salida - entrada) por tramo (algoritmo R3).</summary>
    public short Minutos()
    {
        int minutos = 0;
        foreach (short m in H)
            minutos = minutos >= 0 ? minutos - m : minutos + m;
        return (short)minutos;
    }

    /// <summary>Texto de la celda: tramos "08:00-15:00 16:00-19:00" o el tipo de día.</summary>
    public string Texto()
    {
        if (T > 0 && T < 100) return TipoTexto();
        var sb = new System.Text.StringBuilder();
        string sep = "-";
        foreach (short minutos in H)
        {
            sb.Append($"{minutos / 60:00}:{minutos % 60:00}").Append(sep);
            sep = sep == "-" ? " " : "-";
        }
        return sb.ToString().TrimEnd('-', ' ');
    }

    public string TipoTexto() => T switch
    {
        0 => "Trabajo",
        1 => "Vac.",
        2 => "Comp.Fest.",
        3 => "Horas Extras",
        4 => "Otros",
        98 => "Resumen",
        99 => "Baja",
        100 => "",
        _ => "Tipo Día No Def."
    };

    /// <summary>Advertencias del control horario (códigos P → texto, como R3).</summary>
    public string Advertencias()
        => string.Join(" ", P.Where(p => p < _tipoInc.Length).Select(p => $"[{_tipoInc[p]}]"));

    /// <summary>Clase CSS de color: turno de mañana/tarde/partido, libre o tipo especial.</summary>
    public string ClaseColor()
    {
        if (T == 100) return "libre";
        if (T > 0) return "tipo";          // vacaciones, baja, otros… (gris suave)
        if (H.Length > 2) return "partido";
        return H.Length > 1 && H[1] < 960 ? "manana" : "tarde";  // salida antes de las 16:00 = mañana
    }
}
