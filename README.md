# 💰 Expense Tracker

A cross-platform expense tracking application built using **.NET 9**, following **Clean Architecture**, **CQRS**, and modern software engineering practices.

The application provides a unified backend API consumed by both a **.NET MAUI mobile application** for Android/iOS and a **Blazor Web App** for the web.

---

## 🏗️ Architecture

The application follows a layered **Clean Architecture** approach to keep business logic independent from frameworks, databases, and UI implementations.

```text
                         ┌─────────────────────────┐
                         │     ExpenseTracker.Web  │
                         │    Blazor Web App        │
                         └────────────┬────────────┘
                                      │
                                      │ HTTP/REST
                                      │
                         ┌────────────▼────────────┐
                         │   ExpenseTracker.API    │
                         │   ASP.NET Core Web API   │
                         └────────────┬────────────┘
                                      │
                         ┌────────────▼────────────┐
                         │ ExpenseTracker.Application│
                         │ CQRS / Use Cases         │
                         └────────────┬────────────┘
                                      │
                         ┌────────────▼────────────┐
                         │  ExpenseTracker.Domain  │
                         │ Entities / Business     │
                         │ Rules / Interfaces      │
                         └────────────┬────────────┘
                                      │
                         ┌────────────▼────────────┐
                         │ ExpenseTracker.Infrastructure│
                         │ EF Core / DB / External  │
                         │ Services                 │
                         └────────────┬────────────┘
                                      │
                                      ▼
                                PostgreSQL
```

The **.NET MAUI** application follows the same API-first approach:

```text
ExpenseTracker.Mobile
        │
        │ HTTP/REST
        ▼
ExpenseTracker.API
        │
        ▼
Application
        │
        ▼
Domain
        │
        ▼
Infrastructure
        │
        ▼
Database
```

---

## 📁 Solution Structure

```text
ExpenseTracker.sln
│
├── src/
│   └── ExpenseTracker.API
│
├── domain/
│   ├── ExpenseTracker.Application
│   ├── ExpenseTracker.Domain
│   └── ExpenseTracker.Shared
│
├── infrastructure/
│   └── ExpenseTracker.Infrastructure
│
├── clients/
│   ├── ExpenseTracker.Mobile
│   └── ExpenseTracker.Web
│
├── tests/
│   ├── ExpenseTracker.UnitTests
│   └── ExpenseTracker.IntegrationTests
│
└── docs/
    ├── Architecture
    ├── API
    └── Database
```

### Project Responsibilities

| Project                           | Type                 | Responsibility                                                       |
| --------------------------------- | -------------------- | -------------------------------------------------------------------- |
| `ExpenseTracker.API`              | ASP.NET Core Web API | REST APIs, middleware, authentication, Swagger and DI                |
| `ExpenseTracker.Application`      | Class Library        | CQRS, commands, queries, handlers, DTOs and validators               |
| `ExpenseTracker.Domain`           | Class Library        | Entities, business rules, value objects and repository contracts     |
| `ExpenseTracker.Infrastructure`   | Class Library        | EF Core, repositories, database and external service implementations |
| `ExpenseTracker.Shared`           | Class Library        | Shared DTOs, constants, enums and common models                      |
| `ExpenseTracker.Mobile`           | .NET MAUI            | Android/iOS application                                              |
| `ExpenseTracker.Web`              | Blazor Web App       | Web application                                                      |
| `ExpenseTracker.UnitTests`        | xUnit                | Unit tests                                                           |
| `ExpenseTracker.IntegrationTests` | xUnit                | API, database and repository integration tests                       |

---

# 🚀 Features

The Expense Tracker is designed around the following core features.

### Expense Management

* Add expenses
* Edit expenses
* Delete expenses
* View expense history
* Filter expenses by date
* Filter expenses by category
* Add remarks/notes
* Track payment methods

### Category Management

* Create custom categories
* Edit categories
* Delete categories
* Assign expenses to categories
* Support user-defined categories

Example categories:

```text
Groceries
Bills
EMIs
Travel
Shopping
Entertainment
Healthcare
Others
```

### Budget Management

* Create monthly budgets
* Define category-wise budgets
* Track total monthly budget
* Track spending against budget
* Monitor remaining budget
* Identify categories exceeding their budget

### Dashboard

The dashboard is intended to provide:

```text
Total Budget
       ↓
Total Expenses
       ↓
Remaining Budget
       ↓
Category-wise Spending
       ↓
Monthly Spending Trends
```

### Reports

Planned reporting capabilities include:

* Monthly expense reports
* Category-wise spending
* Budget vs actual spending
* Monthly spending trends
* Export reports

---

# 🛠️ Technology Stack

## Backend

* **.NET 9**
* **ASP.NET Core Web API**
* **C#**
* **Entity Framework Core**
* **PostgreSQL**
* **MediatR**
* **FluentValidation**
* **JWT Authentication**
* **Swagger / OpenAPI**
* **Serilog**

## Web

* **Blazor Web App**
* **.NET 9**
* C#
* HTML
* CSS

## Mobile

* **.NET MAUI**
* Android
* iOS
* MVVM
* CommunityToolkit.MVVM

## Testing

* **xUnit**
* **Moq**
* **FluentAssertions**
* ASP.NET Core integration testing

## Development

* Visual Studio
* Git
* GitHub
* GitHub Actions
* Bruno for API testing

---

# 🧱 Clean Architecture

The dependency direction follows the Clean Architecture principle:

```text
API
 │
 ├──────────────► Application
 │                    │
 │                    ▼
 │                 Domain
 │
 └──────────────► Infrastructure
                       │
                       ▼
                    Database
```

The **Domain** layer remains independent of infrastructure and UI frameworks.

### Domain

Contains the core business concepts:

```text
Entities
Value Objects
Business Rules
Domain Events
Repository Interfaces
Enums
Domain Exceptions
```

### Application

Contains application-specific business use cases:

```text
Commands
Queries
Handlers
DTOs
Validators
Interfaces
CQRS Behaviors
```

### Infrastructure

Contains implementation details:

```text
EF Core
DbContext
Repositories
Database Configuration
Authentication
JWT
External Services
Caching
Logging
```

### API

Acts as the entry point to the backend:

```text
Controllers
Middleware
Dependency Injection
Authentication
Authorization
Swagger
Exception Handling
```

---

# 🔄 CQRS

The Application layer uses **CQRS (Command Query Responsibility Segregation)**.

### Commands

Commands modify application state.

```text
CreateExpenseCommand
UpdateExpenseCommand
DeleteExpenseCommand

CreateBudgetCommand
UpdateBudgetCommand
DeleteBudgetCommand
```

### Queries

Queries retrieve data.

```text
GetExpenseQuery
GetExpensesQuery

GetBudgetQuery
GetBudgetByMonthQuery

GetDashboardQuery
GetMonthlyReportQuery
```

Example flow:

```text
POST /api/expenses
        │
        ▼
CreateExpenseCommand
        │
        ▼
CreateExpenseCommandHandler
        │
        ▼
IExpenseRepository
        │
        ▼
ExpenseRepository
        │
        ▼
EF Core
        │
        ▼
PostgreSQL
```

---

# 🗄️ Database

The application uses **Entity Framework Core** as the ORM.

The initial domain model includes entities such as:

```text
User
 │
 ├── Budget
 │     │
 │     └── BudgetCategory
 │
 └── Expense
        │
        └── Category
```

Example:

```text
User
 │
 ├── Budgets
 │
 ├── Categories
 │
 └── Expenses
```

Database migrations are managed through EF Core.

---

# 🔐 Authentication & Authorization

Authentication is designed around ASP.NET Core Identity and JWT.

```text
Client
   │
   ▼
Login
   │
   ▼
ASP.NET Core API
   │
   ▼
Validate Credentials
   │
   ▼
Generate JWT
   │
   ▼
Client
   │
   ▼
Authorized API Requests
```

The architecture can later be extended with:

* Refresh tokens
* Google authentication
* Apple authentication
* Role-based authorization

---

# 📱 Mobile Application

The mobile client is implemented using **.NET MAUI**.

Architecture:

```text
View
 │
 ▼
ViewModel
 │
 ▼
Service
 │
 ▼
API Client
 │
 ▼
ASP.NET Core API
```

The application targets:

```text
Android
iOS
```

MVVM is used to separate UI concerns from application logic.

---

# 🌐 Web Application

The web client uses **Blazor Web App**.

```text
Blazor Components
        │
        ▼
ViewModels / Services
        │
        ▼
API Client
        │
        ▼
ASP.NET Core Web API
```

Using Blazor allows the web application to use C# throughout the frontend while sharing common .NET concepts and contracts with the backend.

---

# 🧪 Testing

The solution contains separate unit and integration test projects.

## Unit Tests

```text
ExpenseTracker.UnitTests
```

Used to test isolated business logic such as:

* Domain rules
* Application handlers
* Validators
* Services
* Calculations

Example:

```text
CreateExpenseHandler
        ↓
Unit Test
        ↓
Verify Expense Creation
```

## Integration Tests

```text
ExpenseTracker.IntegrationTests
```

Used to verify the interaction between:

```text
API
 ↓
Application
 ↓
Infrastructure
 ↓
Database
```

Examples:

* API endpoint tests
* Repository tests
* EF Core tests
* Database integration tests
* Authentication flow tests

---

# 🔌 API

The backend exposes RESTful APIs.

Example endpoints:

```http
POST   /api/auth/register
POST   /api/auth/login

GET    /api/expenses
GET    /api/expenses/{id}
POST   /api/expenses
PUT    /api/expenses/{id}
DELETE /api/expenses/{id}

GET    /api/categories
POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}

GET    /api/budgets
GET    /api/budgets/{id}
POST   /api/budgets
PUT    /api/budgets/{id}
DELETE /api/budgets/{id}

GET    /api/dashboard
GET    /api/reports/monthly
```

Swagger/OpenAPI is available during development for API exploration and testing.

---

# 🧪 API Testing with Bruno

The project uses **Bruno** for API testing.

Example collection:

```text
Bruno/
│
├── Auth/
│   ├── login
│   └── register
│
├── Budgets/
│   ├── create-budget
│   ├── get-budget
│   ├── update-budget
│   └── delete-budget
│
├── Expenses/
│   ├── create-expense
│   ├── get-expenses
│   ├── update-expense
│   └── delete-expense
│
└── Categories/
    ├── create-category
    └── get-categories
```

Environment variables can be used for values such as:

```text
port-number
budgetId
expenseId
categoryId
```

---

# ⚙️ Getting Started

## Prerequisites

Install the following:

* .NET 9 SDK
* Visual Studio 2022 or later
* PostgreSQL
* Git
* Bruno

For mobile development:

* Android SDK / Emulator
* Xcode for iOS development on macOS

---

## Clone the Repository

```bash
git clone <repository-url>

cd expense-tracker
```

---

## Build the Solution

```bash
dotnet restore

dotnet build
```

---

## Configure Database

Update the connection string in:

```text
ExpenseTracker.API
└── appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ExpenseTracker;Username=postgres;Password=your-password"
  }
}
```

---

## Run Database Migrations

From the solution directory:

```bash
dotnet ef database update \
  --project Infrastructure/ExpenseTracker.Infrastructure \
  --startup-project src/ExpenseTracker.API
```

Adjust the paths if your solution folders differ.

---

## Run the API

```bash
dotnet run --project src/ExpenseTracker.API
```

The API will start on the configured HTTP/HTTPS ports.

Swagger can then be accessed through:

```text
/swagger
```

---

## Run the Web Application

```bash
dotnet run --project clients/ExpenseTracker.Web
```

The Blazor application will connect to the configured API.

---

## Run the Mobile Application

Open:

```text
clients/ExpenseTracker.Mobile
```

in Visual Studio and select:

```text
Android Emulator
```

or:

```text
iOS Simulator
```

Then run the project.

---

# 🔧 Configuration

The Web and Mobile applications should be configured with the API base URL.

Example:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7045"
  }
}
```

For local development:

```text
Mobile/Web
    │
    │
    ▼
https://localhost:7045
    │
    ▼
ExpenseTracker.API
```

---

# 📈 Future Enhancements

The architecture is designed to accommodate additional functionality.

### Planned Features

* [ ] User authentication
* [ ] Expense CRUD
* [ ] Category management
* [ ] Monthly budgets
* [ ] Dashboard
* [ ] Reports
* [ ] Recurring expenses
* [ ] Receipt image upload
* [ ] Expense attachments
* [ ] Push notifications
* [ ] Offline support for MAUI
* [ ] Data synchronization
* [ ] PDF/Excel export
* [ ] Multi-currency support
* [ ] Family/shared accounts
* [ ] Subscription tracking

### AI Features

Future versions can integrate AI capabilities for:

```text
Receipt
   ↓
OCR
   ↓
Expense Extraction
   ↓
AI Categorization
   ↓
Expense
```

Additional possibilities:

* Automatic expense categorization
* Spending analysis
* Budget recommendations
* Monthly financial summaries
* Spending forecasts
* Natural-language expense queries

---

# 🧩 Design Principles

The project follows the following principles:

* Clean Architecture
* SOLID
* Separation of Concerns
* Dependency Inversion
* CQRS
* Domain-Driven Design concepts
* Repository Pattern
* Dependency Injection
* Async/Await
* Automated Testing
* API-first development

---

# 📊 Architecture Goals

The primary goals of the architecture are:

```text
Maintainability
      +
Testability
      +
Scalability
      +
Reusability
      +
Separation of Concerns
```

The backend is designed to serve multiple clients through a single API:

```text
             ┌──────────────┐
             │ MAUI Android │
             └──────┬───────┘
                    │
             ┌──────▼───────┐
             │              │
             │ ASP.NET Core │
             │   Web API    │
             │              │
             └──────┬───────┘
                    │
             ┌──────▼───────┐
             │ PostgreSQL   │
             └──────────────┘
                    ▲
             ┌──────┴───────┐
             │ Blazor Web   │
             │     App      │
             └──────────────┘
```

This allows new clients to be added later without modifying the core business logic.

---

# 🤝 Contributing

Contributions, suggestions, and improvements are welcome.

A typical contribution workflow:

```text
Fork
  ↓
Create Branch
  ↓
Implement Feature
  ↓
Add Unit/Integration Tests
  ↓
Run Tests
  ↓
Create Pull Request
```

---

# 📄 License

This project is intended for learning, experimentation, and portfolio development.

Add the appropriate license here if the repository is published under a specific open-source license.

---

## 👨‍💻 Author

**Abhilash Gupta**

Senior Software Engineer | C# | .NET | WPF | MAUI | Blazor | Clean Architecture | AI/LLM

---

⭐ If you find this project useful, consider giving the repository a star.
