# Etapa 1: Usar uma imagem base do SDK .NET Core 3.1 para build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Definir o diretório de trabalho dentro do container
WORKDIR /src

# Copiar o arquivo .csproj e restaurar as dependências
COPY ["API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"

# Copiar o restante do código da aplicação
COPY . .

# Definir o diretório de trabalho para a aplicação
WORKDIR "/src/API"

# Compilar a aplicação
RUN dotnet build "API.csproj" -c Release -o /app/build

# Etapa 2: Publicar a aplicação
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Etapa 3: Usar a imagem base do runtime para executar a aplicação
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final

# Definir o diretório de trabalho no container
WORKDIR /app

# Copiar os arquivos publicados da etapa de build
COPY --from=publish /app/publish .

# Expor a porta 80 para a API
EXPOSE 80

# Definir o comando de entrada para rodar a aplicação
ENTRYPOINT ["dotnet", "API.dll"]