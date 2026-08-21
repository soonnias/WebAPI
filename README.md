# E-Commerce Web API (Clean Architecture) 🌐

## 📌 About the Project
This is a robust RESTful Web API built with ASP.NET Core, strictly following the **Clean Architecture** principles. The project is designed to be highly scalable, maintainable, and testable by separating concerns into distinct layers. It handles core e-commerce functionalities such as managing products and categories, including search and sorting capabilities.

## 🏗 Architecture Layers
The solution is divided into loosely coupled projects:
* **Domain Layer:** Contains enterprise logic, core entities, and custom exceptions.
* **Application Layer:** Contains business logic, interfaces, and DTOs (Data Transfer Objects).
* **Infrastructure Layer:** Handles external concerns like database access (Repository implementation) and third-party services.
* **API / Presentation Layer:** The entry point of the application, containing RESTful Controllers that route HTTP requests to the Application layer.

## ✨ Key Features & Tech Stack
* **Framework:** .NET 8.0 / ASP.NET Core Web API.
* **Design Patterns:** Repository Pattern, Dependency Injection (DI).
* **Authentication:** Custom authentication service (`AuthService`).
* **Asynchronous Programming:** Fully async/await controllers and services for maximum performance.
* **Error Handling:** Centralized `try/catch` blocks and proper HTTP status code management (e.g., 500 Internal Server Error).

## ⚙️ How to Run
1. Clone the repository to your local machine.
2. Open the solution in **Visual Studio**.
3. Update the database connection string in `appsettings.json` (inside the API project).
4. Set the `API` project as the Startup Project.
5. Run the application (F5). You can test the endpoints using Postman or the built-in Swagger UI (if configured).
