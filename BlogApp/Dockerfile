#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlogApp/BlogApp.csproj", "BlogApp/"]
COPY ["BlogApp.Application/BlogApp.Application.csproj", "BlogApp.Application/"]
COPY ["BlogApp.Domain/BlogApp.Domain.csproj", "BlogApp.Domain/"]
COPY ["BlogApp.Persistance/BlogApp.Persistance.csproj", "BlogApp.Persistance/"]
COPY ["BlogApp.Infra/BlogApp.Infra.csproj", "BlogApp.Infra/"]
RUN dotnet restore "BlogApp/BlogApp.csproj"
COPY . .
WORKDIR "/src/BlogApp"
RUN dotnet build "BlogApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogApp.dll"]