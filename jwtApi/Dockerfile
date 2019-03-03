FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["jwtApi/jwtApi.csproj", "jwtApi/"]
RUN dotnet restore "jwtApi/jwtApi.csproj"
COPY . .
WORKDIR "/src/jwtApi"
RUN dotnet build "jwtApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "jwtApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "jwtApi.dll"]