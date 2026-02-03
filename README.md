📚 TK‑UR‑BOOK — Clean Architecture Book Management API

TK‑UR‑BOOK is a complete backend system for managing books and users, built using Clean Architecture to ensure scalability, maintainability, and clear separation of concerns.
The project is designed to be production‑ready, easy to integrate, and highly customizable for educational platforms or digital library systems.


🧱 Architecture Overview

The project follows a clean, layered architecture consisting of four main layers:
1) Domain Layer

    Contains core business entities: Book, User

    Defines domain rules and interfaces

    Fully independent from external dependencies

2) Application Layer

    Contains:

        DTOs

        Services

        Business logic

        FluentValidation validators

    Responsible for orchestrating domain operations

3) Infrastructure Layer

    Contains:

        Entity Framework Core

        DbContext

        Repository implementations

        Stored Procedures

        Serilog logging

    Handles all database and external system interactions

4) API Layer

    Contains:

        Controllers

        Swagger documentation

        JWT Authentication

        Global exception handling

        Unified API response wrappers

🚀 Features
✔ Books Management

    Add new books

    Update existing books

    Delete books

    Search books using SQL Stored Procedures

    Pagination support for listing books

✔ Users Management

    User registration

    User login

    JWT token generation

    Role‑based access protection

✔ Clean Architecture Principles

    High scalability

    Easy to test and maintain

    Clear separation between layers

✔ API Documentation

    Fully documented using Swagger

    XML comments support

    Interactive API testing

✔ Logging

    Structured logging using Serilog

    Request/response logging

    Error tracking

✔ Error Handling

    Global exception middleware

    Unified error responses

    No internal stack traces exposed

✔ Docker Support

    Ready‑to‑use Dockerfile

    docker‑compose for API + SQL Server

🌟 Unique Feature — Smart Recommendation Engine (SRE)

A custom intelligent feature designed to enhance user experience.
What is it?

A recommendation engine that suggests books to users based on:

    Their reading history

    Search behavior

    Popular books

    Preferred categories

How it works

    User activity is logged in an Activity table

    A rule‑based algorithm analyzes the data

    The system returns 3 recommended books via:
    GET /books/recommend

Why it’s unique

    Adds real value beyond CRUD operations

    Makes the system feel “smart”

    Can be upgraded later to ML‑based recommendations

🛠 Tech Stack

    .NET 9

    Entity Framework Core

    SQL Server

    Serilog

    FluentValidation

    Swagger / OpenAPI

    Docker & Docker Compose

📦 Project Structure
Code

src/
 ├── TKURBOOK.API
 ├── TKURBOOK.Application
 ├── TKURBOOK.Domain
 └── TKURBOOK.Infrastructure

🧪 Running the Project
Using Docker
bash

docker-compose up --build

Manual Run

    Start SQL Server

    Update the connection string

    Run the API project from Visual Studio or CLI

📄 License

This project is open for use, modification, and extension.
