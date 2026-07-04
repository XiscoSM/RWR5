# Genera el instalable de Windows (carpeta autocontenida, sin MSIX) y lo comprime
# en dist\. Para instalar: descomprimir y lanzar R5.App.exe (no requiere runtime).
$ErrorActionPreference = "Stop"
$raiz = Split-Path $PSScriptRoot -Parent
$proyecto = Join-Path $raiz "src\R5.App\R5.App.csproj"

dotnet publish $proyecto -f net10.0-windows10.0.19041.0 -c Release `
    -p:WindowsPackageType=None -p:RuntimeIdentifierOverride=win-x64
if ($LASTEXITCODE -ne 0) { throw "publish falló" }

[xml]$csproj = Get-Content $proyecto
$version = ($csproj.Project.PropertyGroup.ApplicationDisplayVersion | Where-Object { $_ }) | Select-Object -First 1

$publicado = Join-Path $raiz "src\R5.App\bin\Release\net10.0-windows10.0.19041.0\win-x64\publish"
$dist = Join-Path $raiz "dist"
New-Item -ItemType Directory -Force $dist | Out-Null
$zip = Join-Path $dist "R5-win-x64-v$version.zip"
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path (Join-Path $publicado "*") -DestinationPath $zip

Write-Host "Instalable Windows: $zip"
