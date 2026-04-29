# Этап 1: Базовый образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этап 2: Сборка (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем .csproj файлы для восстановления зависимостей (кеширование слоев)
# Мы указываем путь TrustDrop/, так как файл лежит в подпапке
COPY ["TrustDrop/TrustDrop.csproj", "TrustDrop/"]
RUN dotnet restore "TrustDrop/TrustDrop.csproj"

# Копируем всё остальное и собираем
COPY . .
WORKDIR "/src/TrustDrop"
RUN dotnet build "TrustDrop.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап 3: Публикация
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TrustDrop.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этап 4: Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrustDrop.dll"]