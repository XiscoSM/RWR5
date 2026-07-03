using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace R5.Core.Logging;

/// <summary>Entrada de log estructurada (JSON por línea), mismo espíritu que RWR4.</summary>
public sealed record LogEntry(
    DateTime FechaHora,
    string Nivel,
    string Categoria,
    string Mensaje,
    int EventId,
    string? ExcepcionTipo,
    string? ExcepcionMensaje,
    string? StackTrace);

/// <summary>
/// Logger a archivo JSONL con nivel mínimo y rotación simple por tamaño
/// (evolución del FileLoggerProvider de RWR4: sin bloqueos largos ni crecimiento sin límite).
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _rutaArchivo;
    private readonly LogLevel _nivelMinimo;
    private readonly object _candado = new();
    private const long TamanoMaximoBytes = 2 * 1024 * 1024; // 2 MB y rota a .old

    public FileLoggerProvider(string rutaArchivo, LogLevel nivelMinimo = LogLevel.Warning)
    {
        _rutaArchivo = rutaArchivo;
        _nivelMinimo = nivelMinimo;
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(this, categoryName);

    public void Dispose() { }

    private void Escribir(LogEntry entrada)
    {
        lock (_candado)
        {
            try
            {
                RotarSiHaceFalta();
                using var escritor = new StreamWriter(_rutaArchivo, append: true);
                escritor.WriteLine(JsonSerializer.Serialize(entrada));
            }
            catch
            {
                // El log nunca debe tumbar la app.
            }
        }
    }

    private void RotarSiHaceFalta()
    {
        var info = new FileInfo(_rutaArchivo);
        if (info.Exists && info.Length > TamanoMaximoBytes)
        {
            string antiguo = _rutaArchivo + ".old";
            File.Delete(antiguo);
            File.Move(_rutaArchivo, antiguo);
        }
    }

    private sealed class FileLogger : ILogger
    {
        private readonly FileLoggerProvider _proveedor;
        private readonly string _categoria;

        public FileLogger(FileLoggerProvider proveedor, string categoria)
        {
            _proveedor = proveedor;
            _categoria = categoria;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _proveedor._nivelMinimo;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var entrada = new LogEntry(
                DateTime.Now,
                logLevel.ToString(),
                _categoria,
                formatter(state, exception),
                eventId.Id,
                exception?.GetType().Name,
                exception?.Message,
                exception?.StackTrace);

            _proveedor.Escribir(entrada);
        }
    }
}
