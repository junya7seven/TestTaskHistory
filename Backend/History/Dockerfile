FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Backend/Application/Application.csproj Backend/Application/
COPY Backend/Data/Data.csproj Backend/Data/
COPY Backend/Entities/Entities.csproj Backend/Entities/
COPY Backend/Interfaces/Interfaces.csproj Backend/Interfaces/
COPY Backend/History/History.csproj Backend/History/


RUN dotnet restore Backend/History/History.csproj

COPY . .
RUN dotnet build Backend/History/History.csproj

RUN dotnet publish Backend/History/History.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "History.dll"]