🛒 TechXpress E-commerce Platform
TechXpress is a scalable and maintainable e-commerce web application for selling electronics (laptops, mobiles, cameras). Built using ASP.NET Core MVC, it follows NTier Architecture, Repository Pattern, and Unit of Work Pattern to ensure clean separation of concerns and ease of development and maintenance.

🎯 Project Objectives
Develop a full-featured e-commerce platform.
Apply clean architecture principles using NTier design.
Integrate secure user authentication and payment gateways.
Deploy a fully functional system with role-based access.

🧱 Architecture & Design Patterns
NTier Architecture
TechXpress.Web: Presentation Layer (Views, Controllers, JS).
TechXpress.Core: Business Logic Layer (Service classes, rules).
TechXpress.Infrasutructure: Data Access Layer (Repositories, DbContext).
Repository Pattern
Abstracts data access logic and centralizes CRUD operations.
Unit of Work Pattern
Ensures a single transaction across multiple operations.
Dependency Injection
Used for injecting services and promoting testability and loose coupling.

🔧 Technologies Used
ASP.NET Core MVC
Entity Framework Core
ASP.NET Identity for authentication and role management
Stripe API for payment integration

