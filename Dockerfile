# Use the official .NET ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Backend_dotNet.csproj", "./"]
RUN dotnet restore "./Backend_dotNet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Backend_dotNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend_dotNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Backend_dotNet.dll"]
