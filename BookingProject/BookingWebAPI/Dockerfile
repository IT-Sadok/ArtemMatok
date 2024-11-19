FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо весь проєкт
COPY BookingProject/BookingWebAPI/ ./

# Відновлюємо залежності
RUN dotnet restore BookingWebAPI.sln

# Будуємо проєкт
RUN dotnet build BookingWebAPI.sln -c Release -o /app/build

FROM build AS publish
# Публікуємо проєкт
RUN dotnet publish BookingWebAPI.sln -c Release -o /app/publish

FROM base AS final
WORKDIR /app
# Копіюємо опубліковані файли у фінальний контейнер
COPY --from=publish /app/publish .

# Вказуємо точку входу
ENTRYPOINT ["dotnet", "BookingWebAPI.dll"]
