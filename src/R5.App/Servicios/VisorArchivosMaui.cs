using R5.Core.Servicios;

namespace R5.App.Servicios;

/// <summary>Guarda el archivo en la caché de la app y lo abre con el visor del sistema.</summary>
public sealed class VisorArchivosMaui : IVisorArchivos
{
    public async Task AbrirAsync(string nombreArchivo, byte[] contenido)
    {
        string ruta = Path.Combine(FileSystem.CacheDirectory, nombreArchivo);
        await File.WriteAllBytesAsync(ruta, contenido);
        await Launcher.Default.OpenAsync(new OpenFileRequest(nombreArchivo, new ReadOnlyFile(ruta)));
    }
}
