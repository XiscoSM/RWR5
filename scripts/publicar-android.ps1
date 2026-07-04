# Genera el APK Android (firmado con la clave debug: válido para distribución
# interna e instalación manual en las tablets) y lo deja en dist\.
$ErrorActionPreference = "Stop"
$raiz = Split-Path $PSScriptRoot -Parent
$proyecto = Join-Path $raiz "src\R5.App\R5.App.csproj"

dotnet publish $proyecto -f net10.0-android -c Release -p:AndroidPackageFormats=apk
if ($LASTEXITCODE -ne 0) { throw "publish falló" }

$dist = Join-Path $raiz "dist"
New-Item -ItemType Directory -Force $dist | Out-Null
Get-ChildItem (Join-Path $raiz "src\R5.App\bin\Release\net10.0-android\publish") -Filter *-Signed.apk |
    Copy-Item -Destination $dist -Force

Write-Host "APK en: $dist"
