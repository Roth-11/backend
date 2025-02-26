# Explorer Notes API

The Explorer Notes API is a backend service designed to manage notes and user authentication. It provides endpoints for creating, reading, updating, and deleting notes, as well as user registration and login functionalities.

## Project Structure

The project is organized into several directories and files:

- **bin**: Contains compiled binaries.
- **Controllers**:
  - `AuthController.cs`: Handles user authentication (registration and login).
  - `NotesController.cs`: Manages CRUD operations for notes.
- **DataContext**: Contains the database context configurations.
- **DTOs**: Data Transfer Objects for API requests and responses.
- **Migrations**: Database migration files for Entity Framework Core.
  - `20250220130007_InitialCreate.cs`: Initial database schema setup.
  - `20250220135407_AddUsersTable.cs`: Adds the Users table to the database.
- **Models**: Contains the data models and database context.
  - `DataContext.cs`: Database context configuration.
  - `Note.cs`: Model for notes.
  - `User.cs`: Model for users.
- **obj**: Contains temporary files used during the build process.
- **Properties**: Contains project properties and settings.
- **Services**:
  - `ApiClient.cs`: Service for making API requests.
- **Configuration Files**:
  - `.gitignore`: Specifies files and directories to be ignored by Git.
  - `appsettings.Development.json`: Development environment settings.
  - `appsettings.json`: Production environment settings.

## Setup Instructions

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Roth-11/backend.git
