using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Descripción de un módulo de R5 para la pantalla de inicio.</summary>
public sealed record ModuloApp(byte CodMenu, string Titulo, string Ruta, string Icono, bool Implementado);

/// <summary>
/// Mapeo CodMenu (permisos que devuelve la API) → módulo de R5.
/// Whitelist: solo lo implementado navega; el resto se muestra deshabilitado,
/// así el usuario ve qué existe y qué está por llegar.
/// </summary>
public static class CatalogoModulos
{
    // Iconos: nombre de glifo del set propio de la UI (css .icono-*). Sin dependencias externas.
    private static readonly Dictionary<byte, ModuloApp> _modulos = new()
    {
        [3] = new(3, "Inventario", "/inventarios", "inventario", true),
        [5] = new(5, "Ajustes stock", "/ajustesStock", "ajustes-stock", true),
        [6] = new(6, "Cambio almacén", "/cambioAlmacen", "almacen", true),
        [7] = new(7, "Ficha producto", "/fichaProducto", "producto", true),
        [8] = new(8, "Pedido proveedor", "/pedidoCompra", "pedido-compra", true),
        [9] = new(9, "Recepción albaranes", "/recepcionAlb", "recepcion", true),
        [11] = new(11, "Traspasos", "/traspasos", "traspasos", true),
        [12] = new(12, "Etiquetas", "/etiquetas", "etiquetas", true),
        [19] = new(19, "Pedido central", "/pedidoCentral", "pedido-central", true),
        [22] = new(22, "Informes", "/informes", "informes", true),
        [24] = new(24, "Preparación", "/preparacion", "preparacion", true),
        [27] = new(27, "Pesar y etiquetar", "/envasado", "balanza", true),
        [33] = new(33, "Horarios", "/horarios", "informes", true),
        [34] = new(34, "Gestiones", "/gestiones", "informes", true),
    };

    /// <summary>Cruza los menús con permiso del usuario con el catálogo de R5.</summary>
    public static IReadOnlyList<ModuloApp> ParaUsuario(IEnumerable<Menu> menus)
    {
        var resultado = new List<ModuloApp>();
        foreach (var menu in menus)
        {
            if (_modulos.TryGetValue(menu.CodMenu, out var modulo))
            {
                // El título de BD prevalece si viene informado (permite renombrar sin recompilar).
                resultado.Add(string.IsNullOrWhiteSpace(menu.DescMenu)
                    ? modulo
                    : modulo with { Titulo = menu.DescMenu });
            }
        }
        return resultado;
    }
}
