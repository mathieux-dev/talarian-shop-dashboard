# TolarianShop Dashboard

## Descrição do Projeto

Este projeto é uma aplicação web desenvolvida como parte de um teste técnico para a TechNation. O objetivo é criar uma ferramenta para gerenciar e visualizar dados financeiros de notas fiscais emitidas. A aplicação inclui um dashboard com indicadores financeiros e uma lista detalhada das notas fiscais, permitindo filtragem e análise.

## Tecnologias Utilizadas

- **Frontend:**
  - HTML, CSS, JavaScript
  - jQuery
  - Bootstrap
  - Chart.js (ou outra biblioteca de gráficos)
  
- **Backend:**
  - C# com .NET 6 (versão LTS)
  - ASP.NET Core MVC
  - Entity Framework Core
  
- **Banco de Dados:**
  - SQL Server
  
- **Infraestrutura:**
  - Docker
  - Docker Compose

## Funcionalidades

### Dashboard
- **Valor Total das Notas Emitidas**: Exibe o valor total de todas as notas fiscais emitidas.
- **Valor das Notas Sem Cobrança**: Mostra o valor total das notas emitidas, mas sem cobrança feita.
- **Valor das Notas Vencidas (Inadimplência)**: Exibe o valor total das notas que não foram pagas dentro do prazo.
- **Valor das Notas a Vencer**: Exibe o valor total das notas que ainda estão no prazo de pagamento.
- **Valor das Notas Pagas**: Exibe o valor total das notas que foram pagas.
- **Gráfico de Evolução da Inadimplência**: Um gráfico que mostra a inadimplência mês a mês.
- **Gráfico de Evolução da Receita**: Um gráfico que mostra a receita recebida mês a mês.

### Lista de Notas Fiscais
- **Nome do Pagador**
- **Número de Identificação da Nota**
- **Data de Emissão**
- **Data da Cobrança**
- **Data do Pagamento**
- **Valor da Nota**
- **Documentos Associados** (Nota Fiscal e Boleto Bancário)
- **Status da Nota** (Emitida, Cobrança Realizada, Pagamento em Atraso, Pagamento Realizado)

### Filtros
- Filtros por mês de emissão, mês de cobrança, mês de pagamento e status da nota.

## Configuração do Ambiente

### Pré-requisitos

- **Docker**
- **Docker Compose**
- **.NET 8 SDK**
- **SQL Server**

### Configuração do Docker

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/TolarianShop Dashboard.git
   cd TolarianShop Dashboard
   ```

2. **Configuração do Docker Compose:**
   - O arquivo `docker-compose.yml` foi configurado para rodar o SQL Server e a aplicação em contêineres separados.

3. **Construir a Imagem Docker:**
   ```bash
   docker-compose build
   ```

4. **Rodar os Contêineres:**
   ```bash
   docker-compose up
   ```

5. **Acessar a Aplicação:**
   - A aplicação estará disponível em `http://localhost:8000`.

## Configuração do Banco de Dados

- As migrações do Entity Framework Core são aplicadas automaticamente na inicialização do contêiner.
- Se precisar rodar as migrações manualmente:
  ```bash
  dotnet ef database update
  ```

## Testes

- **Unitários**: Testes unitários podem ser adicionados na pasta `Tests` usando frameworks como xUnit ou NUnit.
- **Integração**: Testes de integração podem ser escritos para validar a interação entre diferentes partes do sistema.