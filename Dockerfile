
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY fitness/fitness.csproj fitness/
COPY fitness.Application/fitness.Application.csproj fitness.Application/
COPY fitness.Domain/fitness.Domain.csproj fitness.Domain/
COPY fitness.Infrastructure/fitness.Infrastructure.csproj fitness.Infrastructure/

RUN dotnet restore fitness/fitness.csproj

COPY . .

RUN dotnet publish fitness/fitness.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "fitness.dll"]
