
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080:80
EXPOSE 8443:443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserCardsAPI/UserCardsAPI.csproj", "UserCardsAPI/"]
RUN dotnet restore "UserCardsAPI/UserCardsAPI.csproj"
COPY . .
WORKDIR "/src/UserCardsAPI"
RUN dotnet build "UserCardsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserCardsAPI.csproj" -c Release --self-contained false --os linux -o /app/publish-linux  /p:UseAppHost=true 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish-linux .
CMD /app/UserCardsAPI