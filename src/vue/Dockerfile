FROM node:17-alpine AS base
WORKDIR /app
COPY --from=publish /Transleet/publish .
EXPOSE 5095
ENTRYPOINT [ "dotnet","Transleet.dll" ]


FROM mcr.microsoft.com/dotnet/sdk:6.0.202-alpine3.15-amd64 AS restore
WORKDIR /Transleet
COPY ./Transleet/Transleet.csproj .
RUN dotnet restore ./Transleet.csproj

FROM restore AS build
WORKDIR /
COPY . .
WORKDIR /Transleet
RUN dotnet build ./Transleet.csproj -c Release

FROM build AS publish
WORKDIR /Transleet
RUN dotnet publish ./Transleet.csproj -c Release -o ./publish


