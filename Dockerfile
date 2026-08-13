# ---- build: full SDK, discarded after publish ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LoreWeaver.csproj .
RUN dotnet restore LoreWeaver.csproj

COPY . .
RUN dotnet publish LoreWeaver.csproj -c Release -o /app/publish --no-restore

# ---- final: ASP.NET runtime only, no SDK/compiler in the shipped image ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# $APP_UID is the non-root user baked into the base image; switching to it
# after COPY (which needs root to write files) keeps the running process unprivileged.
USER $APP_UID

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LoreWeaver.dll"]
