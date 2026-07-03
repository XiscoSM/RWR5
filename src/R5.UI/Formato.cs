using System.Globalization;

namespace R5.UI;

/// <summary>Formateo de presentación común a todas las páginas (es-ES).</summary>
public static class Formato
{
    private static readonly CultureInfo _cultura = CultureInfo.GetCultureInfo("es-ES");

    public static string Moneda(decimal? valor)
        => valor is null ? "—" : valor.Value.ToString("0.00 €", _cultura);

    // La BD usa 01/01/1900 como "sin fecha": cualquier fecha anterior a 1950 se muestra como vacía.
    public static string Fecha(DateTime? fecha)
        => fecha is null || fecha.Value.Year < 1950 ? "—" : fecha.Value.ToString("dd/MM/yy");

    /// <summary>Cantidad de stock: entera para unidades, 3 decimales para peso.</summary>
    public static string Cantidad(decimal? valor, bool decimales = false)
        => valor is null ? "—" : valor.Value.ToString(decimales ? "0.000" : "0.##", _cultura);
}
