FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо всі файли проєкту до контейнера
COPY . . 

# Відновлюємо залежності
RUN dotnet restore BookingWebAPI.sln

# Збираємо додаток
RUN dotnet publish BookingWebAPI.sln -c Release -o /app/publish

# Копіюємо зібрані файли до фінального образу
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingWebAPI.dll"]
