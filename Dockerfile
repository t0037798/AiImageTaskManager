FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AiImageTaskManager.Api/AiImageTaskManager.Api.csproj", "AiImageTaskManager.Api/"]
COPY ["AiImageTaskManager.Application/AiImageTaskManager.Application.csproj", "AiImageTaskManager.Application/"]
COPY ["AiImageTaskManager.Domain/AiImageTaskManager.Domain.csproj", "AiImageTaskManager.Domain/"]
COPY ["AiImageTaskManager.Infrastructure/AiImageTaskManager.Infrastructure.csproj", "AiImageTaskManager.Infrastructure/"]

RUN dotnet restore "AiImageTaskManager.Api/AiImageTaskManager.Api.csproj"

COPY . .

RUN dotnet publish "AiImageTaskManager.Api/AiImageTaskManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

RUN mkdir -p /app/data
RUN mkdir -p /app/wwwroot/images/generated

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "AiImageTaskManager.Api.dll"]