# 3-Tier Architecture Contacts Management Lab 🏗️

A professional-grade demonstration of the **3-Tier Architecture** pattern using C# and .NET. This project serves as a technical laboratory for building decoupled, maintainable, and secure database-driven applications.

## 🏛️ Architectural Design
The solution is strictly divided into three distinct layers to ensure a professional separation of concerns:

### 1. Presentation Layer (`Contacts.ConsoleApp`)
- Responsible for handling user interactions and displaying information.
- Communicates exclusively with the **Business Layer**.

### 2. Business Logic Layer (`Contacts.Business`)
- Acts as a bridge between the UI and Data layers.
- Contains core logic, data validation rules, and business objects (`clsContact`, `clsCountry`).
- Ensures that no invalid data reaches the persistence layer.

### 3. Data Access Layer (`Contacts.Data`)
- Dedicated to database communication and CRUD operations.
- Implements secure **SQL Parameterized Queries** to prevent SQL Injection.
- Handles direct interactions with SQL Server using **ADO.NET**.

---

## 🛠️ Tech Stack & Patterns
- **Language:** C# 10.0+
- **Framework:** .NET Framework / .NET Core
- **Database:** Microsoft SQL Server
- **Architectural Pattern:** 3-Tier Design
- **Data Access:** ADO.NET (Connected & Disconnected models)

## 🚀 Key Technical Features
- **Data Integrity:** Strict validation within the Business Layer.
- **Secure Persistence:** Fully parameterized queries for all database interactions.
- **Relational Mapping:** Clean mapping between SQL tables and C# objects.
- **Scalability:** The design allows for easy swapping of the UI (e.g., from Console to Web) without touching the Data or Business layers.

## ⚙️ Getting Started
1. **Database Setup:** Restore the provided `ContactsDB.bak` file to your local SQL Server instance.
2. **Configuration:** Update the connection string in `Contacts.Data/clsDataAccessSettings.cs` to match your local environment.
3. **Run:** Set `Contacts.ConsoleApp` as the startup project and run.

---

## ⚖️ License
This project is licensed under the MIT License.
