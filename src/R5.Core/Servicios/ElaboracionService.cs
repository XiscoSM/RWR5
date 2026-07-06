using R5.Core.Api;
using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>
/// Elaboración de productos (módulo 32) contra WebApiRW (api/Elaboracion, aditivo).
/// Una elaboración NUEVA parte de fecha 2000-01-01 y número 0: el primer ingrediente
/// insertado crea la elaboración y devuelve su fecha/número reales (así lo hace R3).
/// </summary>
public sealed class ElaboracionService
{
    /// <summary>Fecha "sin elaboración" con la que arranca una elaboración nueva (como R3).</summary>
    public static readonly DateTime FechaNueva = new(2000, 1, 1);

    private readonly ApiWeb _api;

    public ElaboracionService(ApiWeb api)
    {
        _api = api;
    }

    /// <summary>GET Elaboracion/Productos/{codAlm} — productos elaborables del almacén.</summary>
    public Task<ApiRespuesta<List<ElabProducto>>> GetProductosAsync(short codAlm, CancellationToken ct = default)
        => _api.GetAsync<List<ElabProducto>>($"Elaboracion/Productos/{codAlm}", ct);

    /// <summary>GET Elaboracion/List/{codAlm} — elaboraciones del usuario (reg=false abiertas).</summary>
    public Task<ApiRespuesta<List<ElabResumen>>> GetElaboracionesAsync(short codAlm, short codUsuario, bool reg, CancellationToken ct = default)
        => _api.GetAsync<List<ElabResumen>>($"Elaboracion/List/{codAlm}?codUsuario={codUsuario}&reg={reg}", ct);

    /// <summary>GET .../Ingredientes — receta con lo ya acumulado.</summary>
    public Task<ApiRespuesta<List<ElabIngrediente>>> GetIngredientesAsync(int prod, DateTime fecha, int elaboracion, CancellationToken ct = default)
        => _api.GetAsync<List<ElabIngrediente>>($"Elaboracion/{prod}/{fecha:yyyy-MM-dd}/{elaboracion}/Ingredientes", ct);

    /// <summary>GET .../IngredientesIntro — capturas ya introducidas.</summary>
    public Task<ApiRespuesta<List<ElabIngredienteIntro>>> GetIngredientesIntroAsync(int prod, DateTime fecha, int elaboracion, CancellationToken ct = default)
        => _api.GetAsync<List<ElabIngredienteIntro>>($"Elaboracion/{prod}/{fecha:yyyy-MM-dd}/{elaboracion}/IngredientesIntro", ct);

    /// <summary>GET .../PtosCriticos — puntos críticos (APPCC) con rangos.</summary>
    public Task<ApiRespuesta<List<ElabPtoCritico>>> GetPtosCriticosAsync(int prod, DateTime fecha, int elaboracion, CancellationToken ct = default)
        => _api.GetAsync<List<ElabPtoCritico>>($"Elaboracion/{prod}/{fecha:yyyy-MM-dd}/{elaboracion}/PtosCriticos", ct);

    /// <summary>POST Elaboracion/Ingrediente — añade ingrediente (elaboración 0 crea una nueva).</summary>
    public Task<ApiRespuesta<ElabIngredienteResultado>> PostIngredienteAsync(ElabIngredienteInsertDTO alta, CancellationToken ct = default)
        => _api.PostAsync<ElabIngredienteResultado>("Elaboracion/Ingrediente", alta, ct);

    /// <summary>POST .../PtoCritico/{linea}?valorIntro= — guarda el valor medido.</summary>
    public Task<ApiRespuesta> PostPtoCriticoAsync(int prod, DateTime fecha, int elaboracion, int lineaPtoCritico, decimal valorIntro, CancellationToken ct = default)
        => _api.PostAsync(
            $"Elaboracion/{prod}/{fecha:yyyy-MM-dd}/{elaboracion}/PtoCritico/{lineaPtoCritico}?valorIntro={valorIntro.ToString(System.Globalization.CultureInfo.InvariantCulture)}",
            cuerpo: null, ct);

    /// <summary>POST .../Registrar?cant= — cierra la elaboración; devuelve el lote interno.</summary>
    public Task<ApiRespuesta<int>> RegistrarAsync(int prod, DateTime fecha, int elaboracion, decimal cant, CancellationToken ct = default)
        => _api.PostAsync<int>(
            $"Elaboracion/{prod}/{fecha:yyyy-MM-dd}/{elaboracion}/Registrar?cant={cant.ToString(System.Globalization.CultureInfo.InvariantCulture)}",
            cuerpo: null, ct);

    /// <summary>POST .../Eliminar — borra una elaboración abierta.</summary>
    public Task<ApiRespuesta> EliminarAsync(int prod, DateTime fecha, int elaboracion, CancellationToken ct = default)
        => _api.PostAsync($"Elaboracion/{prod}/{fecha:yyyy-MM-dd}/{elaboracion}/Eliminar", cuerpo: null, ct);

    /// <summary>GET Report/EtiqTrazabilidad/{prod}/{lote} — PDF de la etiqueta del lote interno.</summary>
    public Task<ApiRespuesta<byte[]>> GetEtiquetaPdfAsync(int prod, int lote, CancellationToken ct = default)
        => _api.GetBytesAsync($"Report/EtiqTrazabilidad/{prod}/{lote}", ct);
}
