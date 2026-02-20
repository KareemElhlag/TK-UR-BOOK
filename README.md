📚 TK-UR-BOOK: A Real-World Journey into Clean Architecture & DDD

Welcome to TK-UR-BOOK, more than just an E-book store, this is a specialized educational environment designed to mirror the real-world challenges a software engineer faces daily.
🚀 Key Features

    Advanced Identity System: Complete user management including JWT authentication, Refresh Tokens, and secure password recovery 

    Full E-commerce Flow: Seamless book browsing and purchasing integrated with Stripe, handling webhooks securely to finalize orders 

    Engagement Engine: Interactive Rating system and a Smart Favorite "Toggle" mechanism 

    User Activity Tracking: Transparent logging of every user action (Purchases, Ratings, Favorites) for auditing and analytics 

    Role-Based Security: Protected endpoints using customized Roles and Authorization Policies 
🎭 The Scenario: Real-World Engineering

Imagine you've just joined a team and inherited this codebase. Your mission isn't just to add features, but to understand the existing design, fix bugs, and optimize performance.

    The Flow: Requests travel from Queries/Commands through MediatR to reach their final logic 

    Learning by Doing: You might find intentional "design quirks" or naming inconsistencies. This is your playground for Refactoring and improving legacy code 
🧱 Architecture Overview

This project strictly follows Clean Architecture principles to ensure the code remains independent of frameworks and databases:

    Domain Layer: The heart of the system containing Entities, Strongly Typed IDs (BookId, UserId), and Domain Logic 

    Application Layer: Orchestrates business rules using Handlers and the CQRS pattern 
    Infrastructure Layer: Handles external concerns like Data Access (EF Core) and Third-party services (Stripe) 

    Presentation (API): A clean, documented interface via Swagger UI 

🛠️ Tech Stack & Tools

    ASP.NET Core 8: The robust foundation of the API.

    Entity Framework Core: Implementing Soft Delete and Automatic Auditing via AuditTableEntity 

    SQL Server: Reliable storage using bigint for IDs and uniqueidentifier for GUIDs 

    Git & GitHub: Open-source collaboration.

    Scrutor: For elegant, automatic service registration 

🚧 Challenges & Obstacles

During development, we tackled real architectural hurdles:

    DI Resolution: Fixing AggregateException by properly registering complex Handlers into the Dependency Injection container 

    Async Optimization: Mastering the transition from IQueryable to actual data execution using ToListAsync to prevent runtime failures 

📊 Database Schema

The schema is designed for high traceability. It links Ratings, Purchases, Books, and Users while maintaining an audit trail for every change 
🤝 Contribution: This Project is Yours!

This is a community-driven project. Whether you find a design flaw or want to introduce a new feature, we welcome it. The goal is collective learning and mastering the art of debugging and refactoring in a production-like environment.
