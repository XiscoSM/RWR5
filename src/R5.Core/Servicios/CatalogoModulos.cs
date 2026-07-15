using R5.Core.Entidades;

namespace R5.Core.Servicios;

/// <summary>Descripción de un módulo de R5 para la pantalla de inicio.
/// Familia = color del icono en la rejilla (agrupación visual); Orden = posición en la rejilla.</summary>
public sealed record ModuloApp(byte CodMenu, string Titulo, string Ruta, string Icono, bool Implementado,
    string Familia = "teal", int Orden = 0);

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
        // Stock y movimientos (teal)
        [9] = new(9, "Recepción albaranes", "/recepcionAlb", "recepcion", true, "teal", 10),
        [11] = new(11, "Traspasos", "/traspasos", "traspasos", true, "teal", 11),
        [5] = new(5, "Ajustes stock", "/ajustesStock", "ajustes-stock", true, "teal", 12),
        [3] = new(3, "Inventario", "/inventarios", "inventario", true, "teal", 13),
        // Pedidos (azul)
        [19] = new(19, "Pedido central", "/pedidoCentral", "pedido-central", true, "blue", 20),
        [8] = new(8, "Pedido proveedor", "/pedidoCompra", "pedido-compra", true, "blue", 21),
        [24] = new(24, "Preparación", "/preparacion", "preparacion", true, "blue", 22),
        [17] = new(17, "Pedido cliente", "/pedidoCliente", "usuario", true, "blue", 23),
        // Etiquetado (ámbar)
        [12] = new(12, "Etiquetas", "/etiquetas", "etiquetas", true, "amber", 30),
        [27] = new(27, "Pesar y etiquetar", "/envasado", "balanza", true, "amber", 31),
        [31] = new(31, "Despiece", "/despiece", "despiece", true, "amber", 32),
        [32] = new(32, "Elaboración", "/elaboracion", "elaboracion", true, "amber", 33),
        // Consulta (morado)
        [7] = new(7, "Ficha producto", "/fichaProducto", "producto", true, "purple", 40),
        [22] = new(22, "Informes", "/informes", "informes", true, "purple", 41),
        [33] = new(33, "Horarios", "/horarios", "horarios", true, "purple", 42),
        // Sistema (gris)
        [6] = new(6, "Cambio almacén", "/cambioAlmacen", "almacen", true, "slate", 50),
        [34] = new(34, "Gestiones", "/gestiones", "gestiones", true, "slate", 51),
        [35] = new(35, "IFA Central", "/ifa", "ifa", true, "slate", 52),
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
        // Orden fijo por familia: la rejilla sale siempre agrupada, sin depender del orden de la API.
        resultado.Sort((a, b) => a.Orden.CompareTo(b.Orden));
        return resultado;
    }
}
