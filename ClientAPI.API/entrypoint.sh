#!/bin/bash
set -e

# Aguardar o MySQL estar pronto para aceitar conexões
echo "Aguardando o MySQL estar pronto..."
until mysqladmin ping -h"mysql" --silent; do
  echo "Aguardando o MySQL..."
  sleep 1
done

# Aplicar migrations
echo "Aplicando migrations..."
dotnet ef database update --project /app/ClientAPI.Infrastructure

# Iniciar a aplicação
exec dotnet /app/ClientAPI.API.dll
