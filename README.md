# WEX Assessment

This project is an API for managing purchase transactions, including support for currency conversion using fiscaldata exchange rates..

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (opcional, to run container)

---

## locally

1. Clone:

```bash
git clone https://github.com/cesarasbrito/WexAssessment.git
cd WexAssessment
```
2.
```bash
dotnet restore
dotnet build
```
3.
```bash
dotnet run --project Wex.Api
```

## docker
1.
```bash
docker-compose build
```
2.
```bash
docker-compose up
```


## create transactions
```bash
curl --location 'http://localhost:8080/api/transactions' \
--header 'Content-Type: application/json' \
--data '{
    "description": "test transaction",
    "transactionDate": "2025-07-31T15:30:00",
    "amountUsd": 10000.977
}'
```

## retrieve
```bash
curl --location 'http://localhost:8080/api/transactions/<idtranasctio>?country=Brazil-Real' \
--header 'Content-Type: application/json'
```


Project Structure

Wex.Api — business API

Wex.Application — Application services

Wex.Domain — Entities and business rules

Wex.Infrastructure — Repository implementations, database access, and external clients

Wex.Tests — Automated testing
