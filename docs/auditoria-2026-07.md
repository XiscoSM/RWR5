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

## P1 — aplicados (commits `5a2f3b8`…`c202d63`, lotes A–F)

Hecho y verificado en la app:
- **Errores de carga dejan de disfrazarse de "sin datos"**: componente `Aviso` con botón *Reintentar* (`OnReintentar`); FichaProducto (EANs y stock de tiendas distinguen fallo de vacío), Gestiones (catálogos), AjustesStockAlta/InformesAlta (spinner de tipos ya no eterno + AjustesStock bloquea si fallan los programas), PedidoCentralAlta (centros con reintentar), InformesDetalle (fallo de cabecera avisa en vez de ocultar columnas).
- **Datos**: cajas coherentes con la cantidad al grabar (recepción, compra, picking); cambiar de centro en PedidoCentralAlta invalida la ficha cargada.
- **UX**: foco del escáner tras elegir tipo/gama (4 altas) y a la contraseña en Login; RecepcionAlbAlta ya no falla en silencio sin nº de albarán; logout en dos pasos; `Visor.AbrirAsync` con try/catch (Horarios, Envasado); Login limpia pwd tras error y deshabilita Entrar sin enrolar.
- **Formato/responsive**: barrido de fechas de display a `Formato.Fecha` (oculta 01/01/1900); tablas con scroll horizontal (`.r5-tabla-scroll`); barra superior no desborda <720px.

## P1 — aplicados (2ª tanda, additivo sin procs nuevos)

Hecho y compilado (0 err/0 warn, 16 tests verdes); verificación interactiva de escaneo pendiente de QA en TEST:
- **Lista de líneas visible en las 7 altas** (`PanelLineas` genérico): tras cada añadido se recarga del endpoint de líneas ya existente y se muestra en tabla. Da visibilidad de lo capturado; la corrección de cantidad se hace re-escaneando el producto (el POST es InsertUpdate). Inventario muestra "Contado en este terminal".
- **Cabecera real del servidor en los 4 detalles con endpoint** (PedidoCompra `GET /{num}`, RecepcionAlb `GET /{num}/{fecha}`, Ajuste `GET /{num}/{fecha}`, Informe `GET /{num}/Informe`): el estado (`Reg`/`Estado`) del servidor manda sobre el `?reg=` de la bandeja → deep-link robusto (badge + acción correctos sin query). Cargada en paralelo con las líneas.
- **Truncado**: bandejas Recepción y Ajustes suben `top` 50→300 y avisan ("Mostrando los 300 más recientes…") cuando se llega al tope.

## P1 — pendiente (requiere API/proc nuevo o decisión de producto)

**Funcional / datos**
- Cabecera de **PedidoCentral** y **Traspasos**: no existe proc de cabecera por número (los otros 4 sí). Siguen con `?reg=`. Requiere `PROC_…Cab_Select` nuevo.
- "Total" del conteo global multi-terminal: el panel por-terminal ya da visibilidad honesta ("este terminal"); el total real entre terminales necesita endpoint/proc.
- InventariosConteo: conteo aditivo sin "corregir último" (el tope de sanidad ya evita lo catastrófico).
- Bandeja **PedidoCompra**: el `top=50` lo impone el servidor sin parámetro; subirlo necesita que la API acepte `top`.

**Completitud (usable de extremo a extremo)**
- Borrar/anular una línea desde las altas: hoy solo se corrige re-escaneando (sobrescribe). El borrado real necesita endpoint DELETE de línea + proc.
- ~~Recepción contra pedido de compra~~ **HECHO (client-side)**: en RecepcionAlbAlta se elige el pedido (tecleando el nº o de la lista de pendientes del proveedor) y se confirma viendo sus productos; el `NumPedido` viaja al Get de producto (precarga cantidad pedida) y a la línea. La API/proc ya lo soportaban.
- PedidoCentralDetalle de pedido abierto: sin editar/borrar líneas.
- PreparacionDetalle: no se puede revisar el contenido del pool sin asignárselo; asignación puede "robar" trabajo sin confirmación.
- CambioAlmacen: exige saberse el número de memoria (sin lista de almacenes permitidos).
- Gestiones: firma sin poder ver el documento; identificación sin PIN (decisión de negocio a confirmar).
- InventariosDetalle: solo conteos del terminal propio — el doble conteo entre terminales (el caso real) es invisible.

**UX / consistencia (restante)**
- RecepcionAlbAlta / PedidoCompraAlta: el nº de albarán / proveedor queda incorregible tras la primera línea (`disabled` cuando ya hay documento). Permitir editar con confirmación.
- Horarios vista "Actual" no muestra de qué semana es el cuadro (necesita el rango de fechas de la API).

## P2 — aplicados

- **Jerarquía**: "Buscar" pasa a secundario en las 7 altas (el escáner ya hace submit; Añadir/Registrar quedan como únicos primarios).
- **Estados vacíos con CTA**: "+ Nuevo…" dentro del vacío en las bandejas de Recepción, Ajustes, PedidoCompra y Traspasos.
- **Accesibilidad**: `aria-label` en los botones icon-only de la barra (Inicio, Salir); `aria-pressed` en los segmentos (bandejas, Etiquetas, Preparación); `:focus-visible` global en botones/segmentos/filas.
- **Etiquetas**: cambiar de modo limpia el producto cargado (evita solicitar en el modo equivocado); la tarjeta de cola solo se muestra en modo PDA.
- **Envasado**: tope de sanidad al peso (≥100 kg); los caducados quedan deshabilitados y atenuados (no invitan a pulsar); foco al peso tras elegir producto y tras etiquetar.
- **Login**: redirige a /inicio si ya hay sesión (evita re-loguear con el botón atrás).
- **FichaProducto**: sonido de éxito al identificar por escaneo.
- **Preparación**: botón "Actualizar" (el pool cambia según trabajan otros; la lista no se auto-refresca).
- **`type="number"` → `type="text" inputmode="numeric"` en los campos ENTEROS** (cajas, nº almacén, terminal/puerto/idHard, empleado, usuario): quita spinners y "e" en el WebView. Los decimales (cantidad/peso/precio) se dejan como `number` por el riesgo de coma es-ES.
- **Guard sin conexión centralizado en `BotonConfirmar`**: deshabilita el registro irreversible y avisa cuando no hay red — cubre todos los Registrar/Vaciar/Enviar en un solo componente (reacciona a `IEstadoRed`).
- **Filtro por proveedor en la bandeja de recepción** (en memoria: nombre, código o nº de albarán del proveedor).

## P2 — deferido (con motivo)

- MainLayout: título por switch → CascadingValue. **Recomendado NO hacer**: el switch central ya renderiza todos los títulos; el refactor sería churn en ~30 ficheros (o dos fuentes de verdad) sin cambio visible. Solo merece la pena si se quiere título dinámico por página (p. ej. mostrar el nº de documento en la barra).

## Justificación de posiciones (área 4 del encargo)

Patrones correctos que se mantienen como norma: cabecera con "← Volver" izquierda +
título + acciones de documento a la derecha; acción de zona de trabajo como botón
de bloque al pie de la tarjeta (alcance de pulgar); segmento de filtros a la
izquierda, alta "+ Nuevo" a la derecha; PDF siempre secundario (se corrigió
TraspasosDetalle, que lo tenía primario). Desvíos detectados y pendientes: los tres
primarios simultáneos en altas (Registrar/Buscar/Añadir) diluyen la jerarquía;
botones de solicitud de Gestiones comparten fila con los filtros siendo acciones de
naturaleza distinta.
