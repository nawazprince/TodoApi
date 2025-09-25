# Todo REST API – .NET 8

A full-featured Todo REST API built with .NET 8 Web API, Entity Framework Core and MS SQL Server.
It demonstrates best practices for modern ASP.NET development—secure authentication, repository pattern, DTO/entity mapping with AutoMapper, and robust error handling.

🚀 Features

✅ Authentication APIs

1. POST /api/auth/signup – User registration
2. POST /api/auth/login – User login with JWT token response


✅ Protected Todo APIs (Require JWT Authentication)

1. GET /api/todos - Get the user's todos with pagination
2. GET /api/todos/{id} - Get specific todo by ID
3. POST /api/todos - Create new todo
4. PUT /api/todos/{id} - Update existing todo
5. DELETE /api/todos/{id} - Delete todo

## NuGet Packages

1. Microsoft.EntityFrameworkCore.SqlServer
2. Microsoft.EntityFrameworkCore.Tools
3. AutoMapper.Extensions.Microsoft.DependencyInjection
4. Microsoft.AspNetCore.Authentication.JwtBearer


## Prerequisites

Before running the project, make sure you have:

1. .NET 8 SDK
2. SQL Server

## Setup Instructions

1. Clone the repository
2. Configure the database connection. Update appsettings.json with your SQL Server connection string:
3. Run the application. It will be running on http://localhost:5001 which is mensioned in `launchSettings.json file.`

## Postman collection

Import the postman collection `Todo API.postman_collection.json`