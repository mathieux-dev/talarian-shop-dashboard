#!/bin/bash
set -e

# Defina vari�veis para a string de conex�o
CONNECTION_STRING="your_connection_string_here"

# Crie o banco de dados se n�o existir
echo "Criando o banco de dados (se n�o existir)..."
dotnet ef database update --project /app/TolarianShop.csproj --context ApplicationDbContext --connection "$CONNECTION_STRING"

# Aplicar migrations
echo "Aplicando migrations..."
dotnet ef database update --project /app/TolarianShop.csproj --context ApplicationDbContext --connection "$CONNECTION_STRING"

# Iniciar a aplica��o
echo "Iniciando a aplica��o..."
dotnet TolarianShop.dll
