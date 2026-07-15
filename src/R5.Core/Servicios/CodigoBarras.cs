namespace R5.Core.Servicios;

/// <summary>Resultado de interpretar un código de barras escaneado (port de EanProdLote de R3).</summary>
public sealed class CodigoEscaneado
{
    public long Ean13_14 { get; init; }
    public int Prod { get; init; }
    public int Lote { get; init; }
    public decimal Cant { get; init; }
    public decimal Importe { get; init; }

    public bool EsEan => Ean13_14 > 0;
    public bool EsProdLote => Prod > 0 && Lote > 0;
    /// <summary>EAN de balanza/peso variable: trae la cantidad (kg) embebida.</summary>
    public bool TraeCantidad => Cant > 0;
}

/// <summary>
/// Utilidades de códigos de barras (port fiel de EanProdLote/ValidaEAN13_14 de R3):
/// dígito de control EAN13/14 y decodificación de EANs compuestos de balanza y
/// trazabilidad (16 = prod+lote; 19 = prod+peso+importe; 28 = prod+peso+importe+lote).
/// </summary>
public static class CodigoBarras
{
    /// <summary>Valida el dígito de control de un EAN13/EAN14 (rellena con ceros a 14).</summary>
    public static bool ValidaEan13_14(string numero)
    {
        numero = numero.Trim();
        if (numero.Length is < 5 or > 14 || !numero.All(char.IsAsciiDigit)) return false;
        numero = numero.PadLeft(14, '0');

        int ultimo = numero[^1] - '0';
        // Dígito de control sobre las 13 primeras cifras (pesos 3/1 desde la derecha).
        int mult = 3, suma = 0;
        for (int i = 12; i >= 0; i--)
        {
            suma += (numero[i] - '0') * mult;
            mult = mult == 1 ? 3 : 1;
        }
        return (10 - (suma % 10)) % 10 == ultimo;
    }

    /// <summary>
    /// Interpreta un código escaneado como hacía R3 (EanProdLote): quita un posible
    /// carácter no numérico inicial del escáner y decodifica según longitud.
    /// Devuelve null si el código no encaja en ningún formato conocido.
    /// </summary>
    public static CodigoEscaneado? TryParse(string txt)
    {
        txt = txt.Trim();
        if (txt.Length > 0 && !char.IsAsciiDigit(txt[0])) txt = txt[1..];
        if (txt.Length == 0 || !txt.All(char.IsAsciiDigit)) return null;

        // EAN de caja de proveedor con prefijo GS1 01/02: el EAN14 real va detrás
        // (los de longitud 16 pueden confundirse con trazabilidad: ahí gana trazabilidad).
        if (txt.Length > 16 && txt.Length <= 18 && (txt.StartsWith("01") || txt.StartsWith("02"))
            && long.TryParse(txt.Substring(2, 14), out long ean14))
        {
            return ValidaEan13_14(txt.Substring(2, 14)) ? new CodigoEscaneado { Ean13_14 = ean14 } : null;
        }

        return txt.Length switch
        {
            < 15 => long.TryParse(txt, out long ean) && ValidaEan13_14(txt)
                ? new CodigoEscaneado { Ean13_14 = ean } : null,
            16 => new CodigoEscaneado
            {
                Prod = int.Parse(txt[..8]),
                Lote = int.Parse(txt[8..])
            },
            19 => new CodigoEscaneado
            {
                Prod = int.Parse(txt[..8]),
                Cant = decimal.Parse(txt.Substring(8, 5)) / 1000m,
                Importe = decimal.Parse(txt.Substring(13, 6)) / 100m
            },
            28 => new CodigoEscaneado
            {
                Prod = int.Parse(txt[..8]),
                Cant = decimal.Parse(txt.Substring(8, 5)) / 1000m,
                Importe = decimal.Parse(txt.Substring(13, 6)) / 100m,
                Lote = int.Parse(txt.Substring(19, 9))
            },
            _ => null
        };
    }
}
