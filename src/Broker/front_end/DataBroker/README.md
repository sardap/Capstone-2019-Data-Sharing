# Data Broker

## Set up Development Environment

* Data broker uses different connection string based on an environment variable: `ASPNETCORE_ENVIRONMENT`.
* There are currently 2 connection strings set in appsettings.json

|ASPNETCORE_ENVIRONMENT|Connection string|
|---|---|
|Development|DefaultConnection|
|Production (default)|AwsConnection| 

Please set the `ASPNETCORE_ENVIRONMENT` accordingly.

If you would like your data broker to connect to a different connection string for development or testing purposes, you can change the `DefaultConnection` value and set the `ASPNETCORE_ENVIRONMENT` to `Development`.