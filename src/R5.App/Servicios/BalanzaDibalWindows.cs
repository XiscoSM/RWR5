#if WINDOWS
using System.IO.Ports;
using Microsoft.Extensions.Logging;
using R5.Core.Config;
using R5.Core.Hardware;

namespace R5.App.Servicios;

/// <summary>
/// Báscula Dibal D-POS por puerto serie (protocolo "caja registradora" portado de R3):
///   envío  STX '0' '1' ESC PPPPP ETX   → respuesta ACK
///   envío  ENQ                         → respuesta STX '0' '2' ESC '3' ESC WWWWW ESC PPPPP ESC IIIIII ETX
/// WWWWW = peso en gramos. 9600 baudios, paridad impar, 7 bits, 1 stop.
/// El retardo entre operaciones es necesario con adaptadores USB (como en R3).
/// </summary>
public sealed class BalanzaDibalWindows : IBalanza, IDisposable
{
    private readonly ConfiguracionApp _config;
    private readonly ILogger<BalanzaDibalWindows> _logger;
    private SerialPort? _puerto;
    private decimal _ultimoPeso;

    private const int RetardoUsbMs = 100;

    public BalanzaDibalWindows(ConfiguracionApp config, ILogger<BalanzaDibalWindows> logger)
    {
        _config = config;
        _logger = logger;
    }

    public bool Disponible => _config.PuertoBalanza > 0;

    public bool Conectada => _puerto?.IsOpen == true;

    public event EventHandler<LecturaPeso>? PesoRecibido;

    public Task<bool> ConectarAsync(CancellationToken ct = default)
    {
        if (!Disponible) return Task.FromResult(false);
        try
        {
            string nombre = "COM" + _config.PuertoBalanza;
            if (!SerialPort.GetPortNames().Any(p => p.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("Puerto {Puerto} no existe", nombre);
                return Task.FromResult(false);
            }

            _puerto?.Dispose();
            _puerto = new SerialPort(nombre, 9600, Parity.Odd, 7, StopBits.One)
            {
                ReadTimeout = 1000,
                WriteTimeout = 2000
            };
            _puerto.Open();
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error abriendo el puerto de la báscula");
            return Task.FromResult(false);
        }
    }

    public Task DesconectarAsync()
    {
        try { _puerto?.Close(); } catch { /* cierre defensivo */ }
        return Task.CompletedTask;
    }

    /// <summary>Sondea la báscula hasta obtener un peso nuevo estable o agotar ~8s.</summary>
    public Task<LecturaPeso?> LeerPesoAsync(CancellationToken ct = default)
        => Task.Run<LecturaPeso?>(() =>
        {
            if (_puerto is null || !Conectada) return null;

            var limite = DateTime.UtcNow.AddSeconds(8);
            while (!ct.IsCancellationRequested && DateTime.UtcNow < limite)
            {
                try
                {
                    decimal? peso = CicloLectura();
                    if (peso is > 0 && peso != _ultimoPeso)
                    {
                        _ultimoPeso = peso.Value;
                        var lectura = new LecturaPeso(peso.Value, Estable: true, DateTime.Now);
                        PesoRecibido?.Invoke(this, lectura);
                        return lectura;
                    }
                }
                catch (TimeoutException)
                {
                    // Sin peso nuevo todavía: se reintenta dentro de la ventana.
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error leyendo la báscula");
                    return null;
                }
            }
            return null;
        }, ct);

    private decimal? CicloLectura()
    {
        if (_puerto is null) return null;

        // Precio 0: solo interesa el peso (R5 calcula el importe aparte).
        const char STX = (char)2, ESC = (char)27, ETX = (char)3;
        string peticion = $"{STX}01{ESC}00000{ETX}";

        Thread.Sleep(RetardoUsbMs);
        _puerto.DiscardInBuffer();
        _puerto.Write(peticion);
        Thread.Sleep(RetardoUsbMs);
        if (_puerto.ReadByte() != 6) return null; // esperaba ACK

        Thread.Sleep(RetardoUsbMs);
        _puerto.Write(((char)5).ToString()); // ENQ
        Thread.Sleep(RetardoUsbMs);

        var buffer = new int[25];
        buffer[0] = _puerto.ReadByte();
        if (buffer[0] != 2) return null; // esperaba STX

        // Lectura carácter a carácter (el Read en bloque falla, visto en R3).
        for (int i = 1; i <= 24; i++) buffer[i] = _puerto.ReadChar();
        if (buffer[1] != '0' || buffer[2] != '2' || buffer[24] != 3) return null;

        // Posiciones 6..10 = WWWWW (gramos).
        string gramos = string.Concat(buffer[6..11].Select(c => (char)c));
        return decimal.TryParse(gramos, out decimal valor) ? valor / 1000m : null;
    }

    public void Dispose() => _puerto?.Dispose();
}
#endif
