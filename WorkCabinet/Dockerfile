
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000:80
EXPOSE 5001:443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorkCabinet/WorkCabinet.csproj", "WorkCabinet/"]
RUN dotnet restore "WorkCabinet/WorkCabinet.csproj"
COPY . .
WORKDIR "/src/WorkCabinet"
RUN dotnet build "WorkCabinet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkCabinet.csproj" -c Release --self-contained false --os linux -o /app/publish-linux  /p:UseAppHost=true 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish-linux .
CMD /app/WorkCabinet