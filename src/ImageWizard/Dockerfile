# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImageWizard/ImageWizard.csproj", "ImageWizard/"]
COPY ["ImageWizard.Analytics/ImageWizard.Analytics.csproj", "ImageWizard.Analytics/"]
COPY ["ImageWizard.Core/ImageWizard.Core.csproj", "ImageWizard.Core/"]
COPY ["ImageWizard.Utils/ImageWizard.Utils.csproj", "ImageWizard.Utils/"]
COPY ["ImageWizard.Azure/ImageWizard.Azure.csproj", "ImageWizard.Azure/"]
COPY ["ImageWizard.DocNET/ImageWizard.DocNET.csproj", "ImageWizard.DocNET/"]
COPY ["ImageWizard.Generator/ImageWizard.Generator.csproj", "ImageWizard.Generator/"]
COPY ["ImageWizard.ImageSharp/ImageWizard.ImageSharp.csproj", "ImageWizard.ImageSharp/"]
COPY ["ImageWizard.OpenGraph/ImageWizard.OpenGraph.csproj", "ImageWizard.OpenGraph/"]
COPY ["ImageWizard.SvgNet/ImageWizard.SvgNet.csproj", "ImageWizard.SvgNet/"]
RUN dotnet restore "./ImageWizard/ImageWizard.csproj"
COPY . .
WORKDIR "/src/ImageWizard"
RUN dotnet build "./ImageWizard.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ImageWizard.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageWizard.dll"]