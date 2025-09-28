FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WexAssessment.sln ./
COPY Wex.Api/*.csproj ./Wex.Api/
COPY Wex.Application/*.csproj ./Wex.Application/
COPY Wex.Domain/*.csproj ./Wex.Domain/
COPY Wex.Infrastructure/*.csproj ./Wex.Infrastructure/
COPY Wex.Tests/*.csproj ./Wex.Tests/
RUN dotnet restore


COPY . ./
RUN dotnet publish Wex.Api -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
  CMD curl --fail http://localhost:80/health || exit 1

EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Production

RUN apt-get update && apt-get install -y curl

ENTRYPOINT ["/entrypoint.sh"]