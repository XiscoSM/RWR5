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

## P1 — pendiente (requiere API o decisión de producto)

**Funcional / datos**
- Detalles sin cabecera del servidor (necesita endpoint aditivo en WebApiRW): PedidoCentral/PedidoCompra/RecepcionAlb dependen del estado que les pasa la bandeja (`?reg=`, ya mitigado); lo robusto es cargar la cabecera real (`Reg`, proveedor, importes) y no fiarse del query. Deep-link sin query pierde botón/badge.
- "Total" del aviso de conteo calculado en cliente: con varios terminales contando, miente. Usar el total que devuelva el proc o etiquetar "de este terminal".
- InventariosConteo: conteo aditivo sin "corregir último" (el tope de sanidad ya evita lo catastrófico).
- Bandejas Recepción/PedidoCompra: `top=50` del servidor trunca en silencio; `top=200` en líneas de albarán ídem. Subir el límite o filtrar/avisar.

**Completitud (usable de extremo a extremo)**
- Altas sin lista de líneas añadidas ni borrar/corregir línea (las 4 altas + pedido central): un error de cantidad hoy no tiene corrección en la app.
- Recepción contra pedido de compra: `NumPedido=0` hardcodeado aunque API y UI ya lo soportan a medias — el circuito pedido→recepción queda cojo.
- PedidoCentralDetalle de pedido abierto: sin editar/borrar líneas.
- PreparacionDetalle: no se puede revisar el contenido del pool sin asignárselo; asignación puede "robar" trabajo sin confirmación.
- CambioAlmacen: exige saberse el número de memoria (sin lista de almacenes permitidos).
- Gestiones: firma sin poder ver el documento; identificación sin PIN (decisión de negocio a confirmar).
- InventariosDetalle: solo conteos del terminal propio — el doble conteo entre terminales (el caso real) es invisible.

**UX / consistencia (restante)**
- RecepcionAlbAlta / PedidoCompraAlta: el nº de albarán / proveedor queda incorregible tras la primera línea (`disabled` cuando ya hay documento). Permitir editar con confirmación.
- Horarios vista "Actual" no muestra de qué semana es el cuadro (necesita el rango de fechas de la API).

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
