# Script PowerShell per eseguire test con diverse configurazioni

param(
    [string]$Category = "All",
    [switch]$Coverage,
    [switch]$Verbose
)

Write-Host "🧪 Esecuzione Test TechTutorPlay" -ForegroundColor Cyan

# Ripristina pacchetti
Write-Host "`n📦 Ripristino pacchetti..." -ForegroundColor Yellow
dotnet restore

# Compila
Write-Host "`n🔨 Compilazione..." -ForegroundColor Yellow
dotnet build --no-restore

# Costruisci comando test
$testCommand = "dotnet test --no-build"

if ($Category -ne "All") {
    $testCommand += " --filter `"Category=$Category`""
}

if ($Verbose) {
    $testCommand += " --verbosity detailed"
}

if ($Coverage) {
    $testCommand += " /p:CollectCoverage=true /p:CoverletOutputFormat=opencover,cobertura"
    $testCommand += " /p:CoverletOutput=./TestResults/"
}

# Esegui test
Write-Host "`n🧪 Esecuzione test..." -ForegroundColor Yellow
Invoke-Expression $testCommand

# Genera report coverage se richiesto
if ($Coverage) {
    Write-Host "`n📊 Generazione report coverage..." -ForegroundColor Yellow
    reportgenerator `
        -reports:./TestResults/coverage.cobertura.xml `
        -targetdir:./TestResults/html `
        -reporttypes:Html
    
    Write-Host "`n✅ Report disponibile in: ./TestResults/html/index.html" -ForegroundColor Green
}

Write-Host "`n✅ Test completati!" -ForegroundColor Green