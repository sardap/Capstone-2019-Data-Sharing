FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Fetcher/Fetcher.csproj", "Fetcher/"]
RUN dotnet restore "Fetcher/Fetcher.csproj"
COPY . .
WORKDIR "/src/Fetcher"
RUN dotnet build "Fetcher.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fetcher.csproj" -c Release -o /app

FROM base AS final
RUN apt update && \
    apt install unzip && \
    curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fetcher.dll"]