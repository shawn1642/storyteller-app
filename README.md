# storyteller-app

A multi-user Todo-list web application built in .NET 8.0 as part of the Storyteller take-home assignment.

## Requirements

- .NET 8.0 SDK
- IDE: Visual Studio, VS Code, Rider, or command line
- OS: Cross-platform (Windows/macOS/Linux)

## Setup Instructions

1. Clone the repository:
- git clone https://github.com/your-username/storyteller-app.git
- cd storyteller-app

2. Restore and build the project:
- dotnet build

3. Run the test suite:
- dotnet test Todo.Tests

4. Run the application:
- dotnet run --project Todo
 -Visit the app at: https://localhost:5001 or http://localhost:5000

## Known Issue

- One unit test is intentionally failing (EqualImportance in `WhenTodoItemIsConvertedToEditFields.cs`) and is part of the required work.

## Troubleshooting

**Issue:** SSL error or port already in use  
**Solution:** Run `dotnet dev-certs https --trust` and restart the app

**Issue:** Entity Framework or migration-related errors  
**Solution:**
- Ensure the database is initialized properly
- Run the following if you see errors like `no such table: AspNetUsers`:
    - PowerShell (Windows only):
    - Get-ChildItem -Recurse | Unblock-File
    - dotnet tool restore
    - cd Todo
    - dotnet ef database update
**Issue:** Branch error (e.g, `main` not found)  
**Solution:** Run `git checkout -b main origin/main` to create a local tracking branch for `main`

## Project Structure

- Todo/         — Main web application (MVC pattern)
- Todo.Tests/   — Unit tests using xUnit
- Todo.sln      — Solution file