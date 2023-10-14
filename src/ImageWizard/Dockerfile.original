#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update
RUN apt-get install -y ttf-mscorefonts-installer fontconfig
RUN apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ImageWizard/ImageWizard.csproj", "ImageWizard/"]
COPY ["ImageWizard.SkiaSharp/ImageWizard.SkiaSharp.csproj", "ImageWizard.SkiaSharp/"]
COPY ["ImageWizard.Core/ImageWizard.Core.csproj", "ImageWizard.Core/"]
COPY ["ImageWizard.Utils/ImageWizard.Utils.csproj", "ImageWizard.Utils/"]
COPY ["ImageWizard.MongoDB/ImageWizard.MongoDB.csproj", "ImageWizard.MongoDB/"]
COPY ["ImageWizard.DocNET/ImageWizard.DocNET.csproj", "ImageWizard.DocNET/"]
COPY ["ImageWizard.SvgNet/ImageWizard.SvgNet.csproj", "ImageWizard.SvgNet/"]
COPY ["ImageWizard.Azure/ImageWizard.Azure.csproj", "ImageWizard.Azure/"]
COPY ["ImageWizard.Analytics/ImageWizard.Analytics.csproj", "ImageWizard.Analytics/"]
COPY ["ImageWizard.ImageSharp/ImageWizard.ImageSharp.csproj", "ImageWizard.ImageSharp/"]
RUN dotnet restore "ImageWizard/ImageWizard.csproj"
COPY . .
WORKDIR "/src/ImageWizard"
RUN dotnet build "ImageWizard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImageWizard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageWizard.dll"]