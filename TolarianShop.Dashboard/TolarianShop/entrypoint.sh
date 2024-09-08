#!/bin/bash
set -e

# Defina variáveis para a string de conexão
CONNECTION_STRING="your_connection_string_here"

# Crie o banco de dados se não existir
echo "Criando o banco de dados (se não existir)..."
dotnet ef database update --project /app/TolarianShop.csproj --context ApplicationDbContext --connection "$CONNECTION_STRING"

# Aplicar migrations
echo "Aplicando migrations..."
dotnet ef database update --project /app/TolarianShop.csproj --context ApplicationDbContext --connection "$CONNECTION_STRING"

# Iniciar a aplicação
echo "Iniciando a aplicação..."
dotnet TolarianShop.dll
