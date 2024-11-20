FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо всі файли
COPY . .

# Відновлюємо залежності
RUN dotnet restore BookingWebAPI.sln

# Створюємо білд
RUN dotnet build BookingWebAPI.sln --configuration Release --no-restore

# Публікуємо білд
RUN dotnet publish BookingWebAPI.sln --configuration Release --no-build -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingWebApi.dll"]
