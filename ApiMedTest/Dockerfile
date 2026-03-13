FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ApiMedTest/ApiMedTest.csproj", "ApiMedTest/"]
COPY ["ApiMedTest.Data/ApiMedTest.Data.csproj", "ApiMedTest.Data/"]
COPY ["ApiMedTest.Domain/ApiMedTest.Domain.csproj", "ApiMedTest.Domain/"]
COPY ["ApiMedTest.Service/ApiMedTest.Service.csproj", "ApiMedTest.Service/"]
RUN dotnet restore "ApiMedTest/ApiMedTest.csproj"

COPY . .
WORKDIR "/src/ApiMedTest"
RUN dotnet build "ApiMedTest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApiMedTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiMedTest.dll"]
