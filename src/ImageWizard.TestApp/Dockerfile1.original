#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ImageWizard.TestApp/ImageWizard.TestApp.csproj", "ImageWizard.TestApp/"]
COPY ["ImageWizard.Analytics/ImageWizard.Analytics.csproj", "ImageWizard.Analytics/"]
COPY ["ImageWizard.Core/ImageWizard.Core.csproj", "ImageWizard.Core/"]
COPY ["ImageWizard.Utils/ImageWizard.Utils.csproj", "ImageWizard.Utils/"]
COPY ["ImageWizard.AWS/ImageWizard.AWS.csproj", "ImageWizard.AWS/"]
COPY ["ImageWizard.Azure/ImageWizard.Azure.csproj", "ImageWizard.Azure/"]
COPY ["ImageWizard.Client/ImageWizard.Client.csproj", "ImageWizard.Client/"]
COPY ["ImageWizard.DocNET/ImageWizard.DocNET.csproj", "ImageWizard.DocNET/"]
COPY ["ImageWizard.FFMpegCore/ImageWizard.FFMpegCore.csproj", "ImageWizard.FFMpegCore/"]
COPY ["ImageWizard.ImageSharp/ImageWizard.ImageSharp.csproj", "ImageWizard.ImageSharp/"]
COPY ["ImageWizard.MongoDB/ImageWizard.MongoDB.csproj", "ImageWizard.MongoDB/"]
COPY ["ImageWizard.OpenCvSharp/ImageWizard.OpenCvSharp.csproj", "ImageWizard.OpenCvSharp/"]
COPY ["ImageWizard.OpenGraph/ImageWizard.OpenGraph.csproj", "ImageWizard.OpenGraph/"]
COPY ["ImageWizard.OpenStreetMap/ImageWizard.OpenStreetMap.csproj", "ImageWizard.OpenStreetMap/"]
COPY ["ImageWizard.Playwright/ImageWizard.Playwright.csproj", "ImageWizard.Playwright/"]
COPY ["ImageWizard.PuppeteerSharp/ImageWizard.PuppeteerSharp.csproj", "ImageWizard.PuppeteerSharp/"]
COPY ["ImageWizard.SkiaSharp/ImageWizard.SkiaSharp.csproj", "ImageWizard.SkiaSharp/"]
COPY ["ImageWizard.SvgNet/ImageWizard.SvgNet.csproj", "ImageWizard.SvgNet/"]
RUN dotnet restore "./ImageWizard.TestApp/./ImageWizard.TestApp.csproj"
COPY . .
WORKDIR "/src/ImageWizard.TestApp"
RUN dotnet build "./ImageWizard.TestApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ImageWizard.TestApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageWizard.TestApp.dll"]