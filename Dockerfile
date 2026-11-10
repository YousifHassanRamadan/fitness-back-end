
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

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet fitness.dll --urls http://0.0.0.0:${PORT:-8080}"]
