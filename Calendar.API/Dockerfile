FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Calendar.API/Calendar.API.csproj", "Calendar.API/"]
COPY ["Calendar.DAL/Calendar.DAL.csproj", "Calendar.DAL/"]
COPY ["Calendar.Domain/Calendar.Domain.csproj", "Calendar.Domain/"]
RUN dotnet restore "Calendar.API/Calendar.API.csproj"
COPY . .
WORKDIR "/src/Calendar.API"
RUN dotnet build "Calendar.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calendar.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calendar.API.dll"]
