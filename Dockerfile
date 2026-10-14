FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["backend/MachineryCRM.Api/MachineryCRM.Api.csproj", "backend/MachineryCRM.Api/"]
COPY ["backend/MachineryCRM.Application/MachineryCRM.Application.csproj", "backend/MachineryCRM.Application/"]
COPY ["backend/MachineryCRM.Domain/MachineryCRM.Domain.csproj", "backend/MachineryCRM.Domain/"]
COPY ["backend/MachineryCRM.Infrastructure/MachineryCRM.Infrastructure.csproj", "backend/MachineryCRM.Infrastructure/"]
RUN dotnet restore "backend/MachineryCRM.Api/MachineryCRM.Api.csproj"

# Copiar código restante e compilar
COPY backend/ backend/
WORKDIR "/src/backend/MachineryCRM.Api"
RUN dotnet publish "MachineryCRM.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MachineryCRM.Api.dll"]