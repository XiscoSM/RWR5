# Auditoría UX/funcional — julio 2026

Auditoría completa de las 33 pantallas (navegación, funcional, UI/UX, jerarquía,
completitud). Los P0 están **aplicados** (commit `6ff47d9`); P1/P2 son el backlog
priorizado.

## ✅ P0 corregidos (verificados en la app)

| # | Hallazgo | Arreglo |
|---|----------|---------|
| 1 | Pantallas sin vía de salida (Gestiones, Horarios, Etiquetas, Ficha) | Botón **Inicio permanente en la barra** en toda página salvo Inicio/Login |
| 2 | Documentos pendientes **atrapados**: traspasos, pedidos proveedor, ajustes e informes sin registrar no podían completarse nunca (detalle solo-consulta, alta siempre crea nuevo) | Los 4 detalles registran pendientes (estado vía `?reg=` desde bandeja, badge Pendiente/Registrado). Recepción ya podía |
| 3 | RecepcionAlbDetalle ofrecía "Registrar" sobre albaranes **ya registrados** | Solo se ofrece con `reg=0`; badge de estado en cabecera |
| 4 | Registros irreversibles a **un toque** (pedido central, 4 altas, Finalizar picking, Vaciar cola) | `BotonConfirmar`: patrón único en dos pasos con resumen ("¿Registrar X con N líneas?…") |
| 5 | EAN escaneado dentro del campo **Cantidad** se grababa como unidades (recepción, compra, inventario…) | Tope de sanidad ≥10.000 en las 7 pantallas de captura, con mensaje explicativo |
| 6 | Picking: coste ilegible → línea grabada con **importe 0 en silencio**; error de carga mostraba "fin de recorrido" e invitaba a Finalizar | Aborta y pide reintentar; estado de error dedicado con botón Reintentar |
| 7 | Envasado: cambiar almacén destino con lote cargado → etiqueta con **precio de un almacén y destino de otro** | El cambio de destino limpia la selección y pide re-escanear |
| 8 | Ajustes (config): Probar/Enrolar mutaban la config viva y **Cancelar no revertía** (la app quedaba apuntando a otro entorno); salir expulsaba a login con sesión activa | Snapshot al entrar + revert en Cancelar; salir vuelve a Inicio si hay sesión |
| 9 | Etiquetas: fallo de red al leer la cola se pintaba como **cola vacía** | Se conserva la cola conocida y se avisa del error |
| 10 | Doble toque en segmentos de bandeja → cargas solapadas y lista del filtro equivocado | Segmentos deshabilitados durante la carga (9 pantallas) |
| 11 | Picking: el foco no volvía al escáner al cambiar de línea | `r5.seleccionar` tras cargar cada línea |

## P1 — siguiente tanda recomendada

**Funcional / datos**
- Errores de red disfrazados de "lista vacía" (patrón `Datos ?? new()` sin mirar `Ok`): FichaProducto (EANs y stock tiendas — se decide traspaso con dato falso), Gestiones (catálogos → un fallo de red parece falta de permiso). Corregido ya en Etiquetas; replicar.
- AjustesStockAlta: fallo de `GetProgramasAsync` ignorado → FK -1 al proc con error críptico; spinner infinito si falla `GetTiposAsync` (mismo en InformesAlta).
- PedidoCentralAlta: fallo de centros → "Cargando centros…" eterno y búsquedas con centro 1 silencioso; cambiar de centro con ficha cargada no recalcula el DTO (stock/alta del centro viejo).
- PedidoCentralDetalle: estado desde query sin verificar contra servidor (deep-link sin query pierde botón/badge; PDF con `_estado ?? 1`). Ideal: endpoint de cabecera (aplica también a RecepcionAlb/PedidoCompra detalles: `Reg`, proveedor, importes).
- Picking: editar cantidad a mano no recalcula cajas (graba pares incoherentes; mismo defecto en RecepcionAlbAlta y PedidoCompraAlta); "total" del aviso de conteo calculado en cliente miente con varios terminales.
- InventariosConteo: conteo aditivo sin "corregir último" (el tope de sanidad ya evita lo catastrófico).
- Bandejas Recepción/PedidoCompra: `top=50` del servidor trunca en silencio; `top=200` en líneas de albarán ídem.
- InformesDetalle: `Ok` de cabecera sin comprobar → columnas ocultas en silencio.
- Login: precarga usuario pero foco en usuario (cada mañana un toque de más); tras error no limpia pwd; Entrar habilitado sin enrolar.

**Completitud (usable de extremo a extremo)**
- Altas sin lista de líneas añadidas ni borrar/corregir línea (las 4 altas + pedido central): un error de cantidad hoy no tiene corrección en la app.
- Recepción contra pedido de compra: `NumPedido=0` hardcodeado aunque API y UI ya lo soportan a medias — el circuito pedido→recepción queda cojo.
- PedidoCentralDetalle de pedido abierto: sin editar/borrar líneas.
- PreparacionDetalle: no se puede revisar el contenido del pool sin asignárselo; asignación puede "robar" trabajo sin confirmación.
- CambioAlmacen: exige saberse el número de memoria (sin lista de almacenes permitidos).
- Gestiones: firma sin poder ver el documento; identificación sin PIN (decisión de negocio a confirmar).
- InventariosDetalle: solo conteos del terminal propio — el doble conteo entre terminales (el caso real) es invisible.

**UX / consistencia**
- Foco inicial del escáner es lotería (autofocus no fiable en WebView): enfocar al mostrar cada buscador (`ElegirGama`/`ElegirTipo`/form de link).
- RecepcionAlbAlta: escaneo sin nº albarán proveedor falla en silencio; `_numAlbProv` incorregible tras la primera línea.
- Ningún error de carga ofrece "Reintentar" (añadir `OnReintentar` opcional al componente Aviso).
- `Formato.Fecha()` no se usa: 21 `ToString("dd/MM/yyyy")` a mano y centinelas 01/01/1900 visibles.
- Responsive: tablas sin `overflow-x` (InformesDetalle 9 columnas rompe en vertical); barra superior sin plan <750px (colapsar chips a iconos).
- Cerrar sesión a un toque sin confirmación, pegado al chip de usuario.
- Horarios/Envasado: `Visor.AbrirAsync` sin try/catch (Traspasos/PedidoCentral sí lo tienen).
- Horarios vista "Actual" no muestra de qué semana es el cuadro.

## P2 — pulido (selección)

- Jerarquía: "Buscar" no debe ser primario en las altas (el escáner ya hace submit); valorar `--peligro` para todos los registros irreversibles o para ninguno.
- Inputs `type="number"` → `type="text" + inputmode` (spinners y "e"/"-" en WebView).
- Estados vacíos con CTA ("+ Nuevo…" dentro del vacío); unificar Etiquetas/Envasado al bloque icono+texto.
- Accesibilidad: `aria-label` en botones icon-only de la barra, `aria-pressed` en segmentos, `:focus-visible` en botones. Contraste de tokens ya pasa AA (verificado).
- MainLayout: título por switch hardcodeado → CascadingValue antes de que crezca el catálogo.
- Banner sin conexión: deshabilitar submits mientras no haya red.
- Refresco manual en bandejas (pool de preparación sobre todo); filtro por proveedor en recepción.
- Etiquetas: cambiar de modo con producto cargado no limpia `_producto`; tarjeta de cola visible en modo "día" donde no aplica.
- Envasado: peso manual sin techo razonable; caducados como botones normales; foco al peso tras etiquetar.
- Login: redirigir a /inicio si ya hay sesión.
- Sonido también al abrir ficha por escaneo (FichaProducto no emite aviso en éxito).

## Justificación de posiciones (área 4 del encargo)

Patrones correctos que se mantienen como norma: cabecera con "← Volver" izquierda +
título + acciones de documento a la derecha; acción de zona de trabajo como botón
de bloque al pie de la tarjeta (alcance de pulgar); segmento de filtros a la
izquierda, alta "+ Nuevo" a la derecha; PDF siempre secundario (se corrigió
TraspasosDetalle, que lo tenía primario). Desvíos detectados y pendientes: los tres
primarios simultáneos en altas (Registrar/Buscar/Añadir) diluyen la jerarquía;
botones de solicitud de Gestiones comparten fila con los filtros siendo acciones de
naturaleza distinta.
