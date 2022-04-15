FROM mcr.microsoft.com/dotnet/sdk:6.0.202-alpine3.15-amd64 AS build
WORKDIR /build
COPY ./src/Transleet/Transleet.csproj .
RUN dotnet restore ./Transleet.csproj
COPY ./src/Transleet/* .
RUN dotnet publish ./Transleet.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0.4-alpine3.15-amd64
WORKDIR /app
COPY --from=build /publish .
EXPOSE 80
ENTRYPOINT [ "dotnet","Transleet.dll" ]