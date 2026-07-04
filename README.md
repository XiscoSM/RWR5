# R5 — cliente RW multiplataforma (tablet + escritorio)

Sucesor de PCRW R3 (WinForms) como cliente de tienda/almacén. App **.NET MAUI Blazor Hybrid**
(.NET 10) con dos targets: **Windows** (escritorio) y **Android** (tablet). Todo el dato viaja
por **WebApiRW** (API REST en producción, auth por API-Key de hardware); no hay acceso directo
a base de datos ni túnel SQL.

## Estructura

| Proyecto | Papel |
|---|---|
| `src/R5.Core` | Capa sin plataforma: cliente API (`ApiWeb`), configuración, entidades (contrato = WebApiRW), servicios, logging, interfaces de hardware (`IBalanza`). |
| `src/R5.UI` | Razor Class Library: páginas y componentes Blazor + sistema de diseño (`wwwroot/r5.css`). Reutilizable como web en el futuro. |
| `src/R5.App` | Host MAUI: BlazorWebView, DI, implementaciones por plataforma (secretos, conectividad). |

## Compilar y ejecutar (inner-loop Windows)

```bash
dotnet build src/R5.App/R5.App.csproj -f net10.0-windows10.0.19041.0
# ejecutar: bin/Debug/net10.0-windows10.0.19041.0/win-x64/R5.App.exe

dotnet build src/R5.App/R5.App.csproj -f net10.0-android
```

## Tests y CI

```bash
dotnet test tests/R5.Core.Tests/R5.Core.Tests.csproj
```

GitHub Actions (`.github/workflows/ci.yml`) ejecuta los tests y compila el host Windows
en cada push/PR. `release.yml` (manual) genera los instalables como artefactos.

## Distribución

```powershell
scripts/publicar-windows.ps1   # → dist/R5-win-x64-vX.zip (autocontenido, sin runtime)
scripts/publicar-android.ps1   # → dist/*.apk (firma debug: distribución interna)
```

La versión visible se controla con `ApplicationDisplayVersion` en `R5.App.csproj`
(se muestra en la pantalla de Ajustes).

## Puesta en marcha de un dispositivo

1. **Ajustes** → elegir entorno (Producción/Beta/Desarrollo/Local/Personalizado) → *Probar conexión*.
2. Número de terminal.
3. **Enrolar** con el IdHard dado de alta en `axe.Hardware_HM1` (la api-key se guarda cifrada:
   Keystore en Android, DPAPI en Windows).
4. Login con usuario + contraseña de RW.

## Reglas del proyecto

- **R4 (WebApiRW) es el canon**: mismos DTOs/procs; de R3 solo se porta lo que R4 no tenga.
- Endpoints nuevos en WebApiRW: **solo aditivos**, por rama + PR, nunca push directo a `main`.
- Hardware tras interfaces (`IBalanza`); se valida en Windows primero.
- Informes RDLC: renderizado server-side (PDF) en WebApiRW.
