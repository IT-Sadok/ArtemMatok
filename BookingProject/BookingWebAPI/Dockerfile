FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Стадія збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо тільки файл рішення та необхідні проєкти для швидкого кешування
COPY BookingProject/BookingWebAPI.sln ./
COPY BookingProject/BookingWebApi/ ./BookingWebApi/
COPY BookingProject/BookingWebApi.Application/ ./BookingWebApi.Application/
COPY BookingProject/BookingWebApi.Domain/ ./BookingWebApi.Domain/
COPY BookingProject/BookingWebApi.Infrastructure/ ./BookingWebApi.Infrastructure/

# Відновлюємо залежності
RUN dotnet restore

# Збираємо проєкт
RUN dotnet publish ./BookingWebApi/BookingWebApi.csproj -c Release -o /app/publish

# Стадія виконання
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingWebApi.dll"]
