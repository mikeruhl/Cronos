FROM microsoft/aspnetcore-build:2 AS build-env
WORKDIR ./Cronos.Web/

# Copy csproj and restore as distinct layers
COPY /Cronos.Web/Cronos.Web.csproj .
RUN dotnet restore Cronos.Web.csproj

# Copy src
COPY /Cronos.Web/. .

# publish -c Release was in doc demo
RUN dotnet publish Cronos.Web.csproj -o /publish 

# Build runtime image
FROM microsoft/aspnetcore:2
COPY --from=build-env /publish /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "Cronos.Web.dll"]