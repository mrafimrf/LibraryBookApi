# LibraryBookApi

A simple RESTful API for managing Books and Authors, built with
**ASP.NET Core** and **Entity Framework Core (SQLite)**.

## 🚀 Features

-   CRUD for Books\
-   CRUD for Authors\
-   EF Core Code‑First + SQLite\
-   Input validation\
-   Logging\
-   Swagger API Documentation

------------------------------------------------------------------------

## 📦 Tech Stack

-   .NET 8
-   ASP.NET Core Web API
-   Entity Framework Core
-   SQLite

------------------------------------------------------------------------

## ▶️ How to Run

### 1. Restore dependencies

    dotnet restore

### 2. Apply EF migrations (if applicable)

    dotnet ef database update

### 3. Run the API

    dotnet run

### 4. Open Swagger UI

http://localhost:5019/swagger

------------------------------------------------------------------------

## 📚 API Endpoints Overview

### Authors

  Method   Endpoint            Description
  -------- ------------------- ------------------
  GET      /api/authors        Get all authors
  GET      /api/authors/{id}   Get author by ID
  POST     /api/authors        Create author
  PUT      /api/authors/{id}   Update author
  DELETE   /api/authors/{id}   Delete author

### Books

  Method   Endpoint          Description
  -------- ----------------- ----------------
  GET      /api/books        Get all books
  GET      /api/books/{id}   Get book by ID
  POST     /api/books        Create book
  PUT      /api/books/{id}   Update book
  DELETE   /api/books/{id}   Delete book

------------------------------------------------------------------------

## 🧪 Sample cURL Request

### Create Author

    curl -X POST http://localhost:5019/api/authors -H "Content-Type: application/json" -d "{ "firstName": "Darma", "lastName": "Darius" }"

### Create Book

    curl -X POST http://localhost:5019/api/books -H "Content-Type: application/json" -d "{ "title": "Stranger Things", "genre": "Action Thriller", "publicationDate": "2025-12-09", "authorIds": [1] }"

------------------------------------------------------------------------

## 📝 Notes

-   Ensure your database file (`library.db`) has proper write
    permissions.
-   Swagger is enabled by default for development environment.

------------------------------------------------------------------------

## © License

MIT License.
