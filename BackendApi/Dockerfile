FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy the project file and restore dependencies
COPY ["BackendApi.csproj", "./"]

RUN dotnet restore "BackendApi.csproj"

# Copy the rest of the code and build the release
COPY . .

RUN dotnet publish "BackendApi.csproj" -c Release -o /app/publish

# Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

# Set the application port
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "BackendApi.dll"]