# Task Management API

A simple and efficient task management API built with .NET 6.0 that allows you to manage tasks and users.

## Prerequisites

Before you begin, ensure you have the following installed on your computer:

1. [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) - Click this link and download the .NET 6.0 SDK for your operating system (Windows/Mac/Linux)
2. [Visual Studio Code](https://code.visualstudio.com/) (Optional but recommended) - A free code editor
3. [Postman](https://www.postman.com/downloads/) - For testing the API

## Step-by-Step Installation Guide

### 1. Download and Install .NET 6.0 SDK
1. Go to https://dotnet.microsoft.com/download/dotnet/6.0
2. Click on the download button for .NET 6.0 SDK
3. Run the installer
4. To verify installation, open Command Prompt (Windows) or Terminal (Mac/Linux) and type:
   ```
   dotnet --version
   ```
   You should see a version number like 6.0.xxx

### 2. Download the Project
1. Download this project as a ZIP file or clone it using Git
2. Extract the ZIP file to a folder of your choice (if downloaded as ZIP)

### 3. Run the Project
1. Open Command Prompt (Windows) or Terminal (Mac/Linux)
2. Navigate to the project folder. For example:
   ```
   cd path/to/TaskManagementAPI
   ```
3. Build the project:
   ```
   dotnet build
   ```
4. Run the project:
   ```
   dotnet run --project TaskManagement.API
   ```
5. The API will start running at `http://localhost:5284`
6. Open your web browser and go to:
   ```
   http://localhost:5284/swagger
   ```
   This will open the Swagger UI where you can test all the API endpoints

## Testing with Postman

1. Open Postman
2. Import the provided Postman collection:
   - Click "Import" in Postman
   - Select the file: `TaskManagement.postman_collection.json`
   - Click "Import"

### Basic API Usage Example:

1. Create a User:
   - In Postman, find "Create User" under the Users folder
   - Click "Send" with this example body:
   ```json
   {
     "username": "john_doe",
     "email": "john@example.com",
     "firstName": "John",
     "lastName": "Doe"
   }
   ```

2. Create a Task:
   - Find "Create Task" under the Tasks folder
   - Click "Send" with this example body:
   ```json
   {
     "title": "My First Task",
     "description": "This is a test task",
     "dueDate": "2024-12-31T00:00:00Z",
     "priority": "Medium",
     "assignedUserId": "user-id-from-step-1"
   }
   ```

## Available API Endpoints

### Tasks
- GET /api/tasks - Get all tasks
- GET /api/tasks/{id} - Get task by ID
- POST /api/tasks - Create new task
- PUT /api/tasks/{id} - Update task
- DELETE /api/tasks/{id} - Delete task
- GET /api/tasks/user/{userId} - Get tasks by user ID

### Users
- GET /api/users - Get all users
- GET /api/users/{id} - Get user by ID
- POST /api/users - Create new user
- PUT /api/users/{id} - Update user
- DELETE /api/users/{id} - Delete user

## Troubleshooting

1. If you see "port already in use" error:
   - Close any other applications that might be using port 5284
   - Or modify the port in `TaskManagement.API/Properties/launchSettings.json`

2. If you see build errors:
   - Run `dotnet restore` to restore packages
   - Ensure you have .NET 6.0 SDK installed

3. If Swagger page doesn't load:
   - Ensure the application is running
   - Try accessing http://localhost:5284/swagger/index.html

For additional help or issues, please create an issue in the repository. 