# StorageApi

A RESTful product inventory API built with ASP.NET Core and Entity Framework Core. StorageApi manages products, supports category filtering, calculates inventory statistics, and uses SQL Server as its backing database.

## Technologies

- ASP.NET Core 10 (`net10.0`)
- Entity Framework Core 10
- SQL Server
- OpenAPI
- Scalar API reference

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server with Windows authentication enabled
- Access to the local SQL Server instance `.`

### Run the API

```bash
dotnet restore
dotnet run --project StorageApi
```

The API uses the `StorageApiContext` connection string configured in `StorageApi/appsettings.json`:

```text
Server=.;Database=StorageApiContext;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True;TrustServerCertificate=True
```

When running in the Development environment, the application automatically applies migrations and seeds the database if it is empty. Development documentation is available at `/openapi/v1.json` and through the Scalar API reference.

## Product model

| Field | Type | Rules |
|---|---:|---|
| `id` | integer | Generated identity; required for updates |
| `name` | string | Required; maximum 150 characters |
| `price` | integer | Minimum 0 |
| `category` | string | Required; maximum 150 characters |
| `shelf` | string | Required; maximum 150 characters |
| `count` | integer | Minimum 0 |
| `description` | string | Required |

## API endpoints

All endpoints use JSON and are served below `/api/Products`.

### Get all products

```http
GET /api/Products
```

Returns every product.

```http
GET /api/Products?category=Electronics
```

Filters products by category. The category comparison is case-insensitive because the query lowercases the supplied value.

**Response:** `200 OK`

```json
[
  {
    "id": 1,
    "name": "Wireless Mouse",
    "price": 25,
    "count": 40,
    "inventoryValue": 1000
  }
]
```

> List and category responses use `ProductDto`, which includes only `id`, `name`, `price`, `count`, and the calculated `inventoryValue` property.

### Get a product

```http
GET /api/Products/{id}
```

| Parameter | Type | Description |
|---|---:|---|
| `id` | integer | Product identifier |

**Responses:**

- `200 OK` — product found
- `404 Not Found` — no product exists with the supplied ID

```json
{
  "id": 1,
  "name": "Wireless Mouse",
  "price": 25,
  "count": 40,
  "inventoryValue": 1000
}

```

### Get inventory statistics

```http
GET /api/Products/stats
```

**Response:** `200 OK`

```json
{
  "totalProducts": 2,
  "totalInventoryValue": 2125,
  "averagePrice": 50
}
```

The response contains:

- `totalProducts`: number of product records
- `totalInventoryValue`: sum of `price × count` for every product
- `averagePrice`: average product price

An empty inventory returns `{ "totalProducts": 0, "totalInventoryValue": 0, "averagePrice": 0 }`.

### Create a product

```http
POST /api/Products
Content-Type: application/json

{
  "name": "USB-C Cable",
  "price": 15,
  "category": "Electronics",
  "shelf": "B3",
  "count": 25,
  "description": "Two-metre USB-C charging cable"
}
```

**Responses:**

- `201 Created` — product created; the response includes the newly created product entity and a `Location` header from `GetProduct`
- `400 Bad Request` — model validation failed

### Update a product

```http
PUT /api/Products/{id}
Content-Type: application/json

{
  "id": 1,
  "name": "Wireless Mouse",
  "price": 30,
  "category": "Electronics",
  "shelf": "A1",
  "count": 35,
  "description": "Ergonomic wireless mouse"
}
```

The `id` in the route must match `id` in the request body.

**Responses:**

- `204 No Content` — product updated
- `400 Bad Request` — route and body IDs do not match
- `404 Not Found` — product does not exist
- `409 Conflict` — concurrency update conflict

### Delete a product

```http
DELETE /api/Products/{id}
```

**Responses:**

- `204 No Content` — product deleted
- `404 Not Found` — no product exists with the supplied ID

## Status codes

| Status | Meaning |
|---:|---|
| `200 OK` | Request succeeded and returned data |
| `201 Created` | A product was created |
| `204 No Content` | Update or delete succeeded without a response body |
| `400 Bad Request` | Invalid request or mismatched update ID |
| `404 Not Found` | Requested product was not found |
| `409 Conflict` | Concurrent database modification detected |

## Database

Entity Framework Core maps `StorageApiContext.Products` to the `Product` table. Database migrations are stored in `StorageApi/Migrations/`.

Development startup:

1. Connects using `StorageApiContext`.
2. Runs EF Core migrations.
3. Seeds two products only if the `Products` table is empty.

The seed products are:

- `Wireless Mouse` — Electronics, shelf A1, price 25, count 40
- `Mechanical Keyboard` — Electronics, shelf A2, price 75, count 15

## Project structure

```text
StorageApi/
├── Controllers/       API endpoints
├── DTOs/              Request/response records and entity mappings
├── Data/              EF Core database context and seed initialization
├── Models/            Product entity
├── Migrations/        EF Core database migrations
├── Properties/        Development launch settings
├── Program.cs         Application startup and service configuration
├── appsettings.json   Database connection and logging configuration
└── StorageApi.http    Example HTTP requests
```
