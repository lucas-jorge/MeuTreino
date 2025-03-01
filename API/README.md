# Comandos 
Comandos utilizados para gerar a API:
```
dotnet new webapi -f netcoreapp3.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.32
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.1.32
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.1.32
dotnet add package MySql.EntityFrameworkCore --version 3.1.32
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.32
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 3.1.32
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 3.1.32
dotnet dev-certs https --trust
```

Após a criação das entidades, rodar o comando a seguir para criar as migrations:
```
dotnet ef migrations add InitialCreation
```

Testando a api:
1º - Acessa a página da API pelo swagger - http://localhost:5000/swagger/index.html
2º - Acessar o método 'api/Usuario/Login'
3º - Informar o usuário e senha no formato:
```
{
  "nome": "Admin",
  "senha": "Admin"
}
```