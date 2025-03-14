# Meu Treino Fitness Tracker

## Description

This project is a backend API built with ASP.NET Core + React for a fitness tracking application. It provides functionalities for managing users and exercises, including user authentication, data validation, and database interactions.

## Features

*   **User Management:**
    *   Create, read, update, and delete user accounts.
    *   User authentication using JWT tokens.
    *   User status management (active, inactive, excluded).
*   **Exercise Management:**
    *   Create, read, update, and delete exercise entries.
    *   Associate exercises with specific users.
*   **Database:**
    *   Uses Entity Framework Core for database interactions.
    *   Supports both MySQL and in-memory databases.
    *   Includes database migrations for schema management.
*   **API Endpoints:**
    *   RESTful API design with clear and well-defined endpoints.
    *   Proper use of HTTP methods (GET, POST, PUT, DELETE).
*   **Authentication:**
    *   Secure user authentication with JWT tokens.
*   **Data Validation:**
    *   Data annotations for basic data validation.
*   **Error Handling:**
    *   Robust error handling with appropriate HTTP status codes.

## Technologies Used

*   ASP.NET Core 3.1
*   Entity Framework Core
*   MySQL
*   JWT (JSON Web Tokens)
*   C#

## Setup Instructions

1.  **Prerequisites:**
    *   .NET Core 3.1 SDK
    *   MySQL Server (optional, if not using in-memory database)
    *   Visual Studio or another IDE

2.  **Clone the Repository:**

    ```
    git clone [your-repository-url]
    cd [your-project-directory]
    ```

3.  **Database Configuration:**

    *   The project supports both MySQL and in-memory databases. To use MySQL, update the connection string in `appsettings.json`.

        ```
        {
          "ConnectionStrings": {
            "DefaultConnection": "Server=your_server;Database=your_database;Uid=your_user;Pwd=your_password;"
          }
        }
        ```

    *   If you prefer to use the in-memory database, no further configuration is needed.

4.  **Apply Migrations:**

    *   If using MySQL, apply the database migrations:

        ```
        dotnet ef database update
        ```

5.  **Run the Application:**

    ```
    dotnet run
    ```

    or in Visual Studio, press the Start button.

## API Endpoints

The API endpoints are structured as follows:

*   `api/usuario`: User management endpoints.
*   `api/exercicios`: Exercise management endpoints.

### User Endpoints

*   `POST /api/usuario/login`: Authenticate a user and retrieve a JWT token.
    *   Request body:

        ```
        {
          "nome": "your_username",
          "senha": "your_password"
        }
        ```

    *   Response:

        ```
        {
          "id": 1,
          "nome": "your_username",
          "token": "your_jwt_token"
        }
        ```

*   `GET /api/usuario?IdUsuario={id}`: Get a specific user by ID.
*   `GET /api/usuario`: Get all users.
*   `PUT /api/usuario`: Update user information.
    *   Request body:

        ```
        {
          "id": 1,
          "nome": "new_username",
          "senha": "new_password",
          "status": 1 // 0: Inativo, 1: Ativo, 2: Excluido
        }
        ```

*   `POST /api/usuario/{id}/exercicios`: Add exercises to a user.
    *   Request body:

        ```
        [
          {
            "exercicio": "Push-ups",
            "serie": 3,
            "repeticoes": 10,
            "tempo": 0
          }
        ]
        ```

*   `GET /api/usuario/{id}/exercicios`: Get exercises for a specific user.

### Exercise Endpoints

*   `GET /api/exercicios/{id}`: Get a specific exercise by ID.
*   `GET /api/exercicios`: Get all exercises.
*   `POST /api/exercicios`: Create a new exercise.
    *   Request body:

        ```
        {
          "exercicio": "Bench Press",
          "serie": 3,
          "repeticoes": 8,
          "tempo": 0
        }
        ```

*   `DELETE /api/exercicios/{id}`: Delete an exercise by ID.

## Authentication

The API uses JWT (JSON Web Tokens) for authentication.  After a successful login, the API returns a token that must be included in the `Authorization` header of subsequent requests.

Example:


The `TokenService.cs` file handles the token generation and validation.  *Important Security Note*: The `Secret` key in `TokenService.cs` *should not* be hardcoded in a production environment.  It should be loaded from a secure configuration source (e.g., environment variables, Azure Key Vault).

## Entities

*   **`TB_USUARIO.cs`:** Represents a user.
    *   Properties: `Id`, `Nome` (Name), `Senha` (Password), `Status` (0: Inativo, 1: Ativo, 2: Excluido), `Created`, `Exercicios`.
    *   `EStatus` enum defines the possible user status values.
*   **`TB_Exercicios.cs`:** Represents an exercise.
    *   Properties: `Id`, `Exercicio` (Exercise Name), `Serie`, `Repeticoes` (Repetitions), `Tempo` (Time).

## Contributing

Please feel free to contribute to the project by submitting pull requests.  Follow the existing code style and conventions.

## License

MIT License


## Equipe

* Lucas Albuquerque Jorge
* Marco Antonio
* Yara Correa
* Tiago Shimizu

## Landing page -> https://lucas-jorge.github.io/MeuTreino.github.io/
## Video -> https://github.com/lucas-jorge/MeuTreino/blob/main/Video/Video%20de%20apresenta%C3%A7%C3%A3o.mp4
## Images
![API](https://github.com/lucas-jorge/MeuTreino/blob/main/Imagens/API.jpg)
![Login page](https://github.com/lucas-jorge/MeuTreino/blob/main/Imagens/Login.jpg)
![APP](https://github.com/lucas-jorge/MeuTreino/blob/main/Imagens/Aplicativo.jpg)
