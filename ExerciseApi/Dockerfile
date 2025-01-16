FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
COPY ["ExerciseApi.csproj", "./"]
RUN dotnet restore "./ExerciseApi.csproj"
COPY . .
WORKDIR "/"
RUN dotnet build "ExerciseApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExerciseApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExerciseApi.dll"]
