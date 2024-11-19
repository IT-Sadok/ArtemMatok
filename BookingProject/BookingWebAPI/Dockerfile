
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Стадія збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо лише файли рішення та проєкту для швидшого кешування
COPY BookingProject/BookingWebAPI.sln ./
COPY BookingProject/BookingWebAPI/ ./BookingWebAPI/

# Відновлюємо залежності
RUN dotnet restore

# Збираємо проєкт
RUN dotnet publish -c Release -o /app/publish

# Стадія виконання
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingWebAPI.dll"]
