# CodeElevate

CodeElevate is a study-oriented ASP.NET Core Web API project created to practice clean implementation habits and solid .NET development fundamentals.

The goal of this repository is not to build a finished product. Its main purpose is to train good .NET practices based on 100 topics from a book that I am currently studying. Each topic is implemented as a small exercise so the project can evolve as a practical learning space.

## Current Purpose

This project is being used as a hands-on environment to:

- practice ASP.NET Core API design
- apply separation of responsibilities
- improve dependency injection usage
- document what was learned from each topic
- reinforce good coding habits through repetition

More topics will be added over time. For now, only Topic 1 is documented.

## Topic Tracking

### Topic 1: Pagination

The first practice topic was **pagination**.

At this stage, the API demonstrates how to:

- validate pagination input
- order data before paginating
- apply `Skip` and `Take`
- return pagination metadata such as total item count and total pages
- expose a paginated endpoint through an ASP.NET Core controller

More practice topics will be added soon.

## How the Project Works

This repository contains the Web API layer of the solution.

The API is configured in [`Program.cs`](/c:/Users/2273129/source/repos/CodeElevate/Program.cs), where the application:

- registers controllers
- enables OpenAPI/Swagger
- adds custom application services through dependency injection
- maps the controller routes

Dependency registration is centralized in [`Extensions/DependenceInjection.cs`](/c:/Users/2273129/source/repos/CodeElevate/Extensions/DependenceInjection.cs), where service and repository abstractions are wired into the container.

The main example currently available is the paginated endpoint in [`Controllers/PraticeController.cs`](/c:/Users/2273129/source/repos/CodeElevate/Controllers/PraticeController.cs). This endpoint:

- receives `page` and `pageSize`
- validates that both values are greater than zero
- requests product data from `IProductService`
- orders the data by `Id`
- applies pagination through the `Paginate<T>` extension method
- returns the paged result to the client

Pagination logic is implemented in [`Extensions/QueryableExtensions.cs`](/c:/Users/2273129/source/repos/CodeElevate/Extensions/QueryableExtensions.cs). The extension method calculates:

- paged data
- total item count
- total page count
- whether there is a next page
- whether there is a previous page

The paginated response model is defined in [`DTOs/PagedResult.cs`](/c:/Users/2273129/source/repos/CodeElevate/DTOs/PagedResult.cs).

## Solution Structure

- `Controllers/`: API endpoints
- `DTOs/`: request and response objects
- `Extensions/`: helper extensions and dependency injection setup
- `Program.cs`: application startup and middleware configuration

This API project also references other solution layers:

- `Application`
- `Infra`

Those projects are responsible for business services and data access used by this API.

## Running the Project

Requirements:

- .NET 9 SDK

Run the API with:

```bash
dotnet run
```

After startup, Swagger UI should be available so you can test the endpoints interactively.

## Notes

This repository is intentionally focused on learning. The implementation may evolve topic by topic as new exercises are added, refactored, and improved over time.
