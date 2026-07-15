namespace R5.Core.Entidades;

/// <summary>Cliente TPV con datos fiscales (módulo 28, facturas simplificadas).</summary>
public sealed class ClienteTpv
{
    public int Cliente { get; set; }
    public string CIF { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string CP { get; set; } = "";
    public string Pais { get; set; } = "ES";
    public string Email { get; set; } = "";
    public string Observaciones { get; set; } = "";
    public string ObservacionPrint { get; set; } = "";
    public byte TipoEnvioFact { get; set; }
    public string UnidTramitadora { get; set; } = "";

    public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();

    /// <summary>Valida NIF/NIE/CIF español (port de ValidarCifCliente de R3).</summary>
    public static bool ValidaCif(string cif)
    {
        cif = cif.Trim().ToUpperInvariant();
        if (cif.Length != 9) return false;

        const string letrasNif = "TRWAGMYFPDXBNJZSQVHLCKE";

        // NIF: 8 dígitos + letra de control.
        if (char.IsAsciiDigit(cif[0]) && cif[..8].All(char.IsAsciiDigit))
            return cif[8] == letrasNif[int.Parse(cif[..8]) % 23];

        // NIE: X/Y/Z + 7 dígitos + letra (X=0, Y=1, Z=2).
        if (cif[0] is 'X' or 'Y' or 'Z' && cif[1..8].All(char.IsAsciiDigit))
        {
            int num = int.Parse((cif[0] - 'X').ToString() + cif[1..8]);
            return cif[8] == letrasNif[num % 23];
        }

        // CIF: letra de tipo + 7 dígitos + control (dígito o letra JABCDEFGHI).
        if (char.IsAsciiLetter(cif[0]) && cif[1..8].All(char.IsAsciiDigit))
        {
            int suma = 0;
            for (int i = 1; i <= 7; i++)
            {
                int d = cif[i] - '0';
                if (i % 2 == 1) { int dbl = d * 2; suma += dbl / 10 + dbl % 10; }
                else suma += d;
            }
            int control = (10 - suma % 10) % 10;
            char letraControl = "JABCDEFGHI"[control];
            return cif[8] == letraControl || cif[8] == (char)('0' + control);
        }
        return false;
    }
}
