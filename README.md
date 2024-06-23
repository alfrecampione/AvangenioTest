### Steps for run each exercice:

- `cd Ex<exercice_number>`
    
- `dotnet run`

### To run the 3rd exercise you must do additional steps:

- Modify the connection string in "appsettings.json" and use yours
- run `dotnet restore` after `cd Ex3` so you can install all de NuGet dependecies

If any changes are applied to the models just run the following commands: 

`dotnet ef migrations add <MigrationName>`

`dotnet ef database update`


## Nugets used in the project

- microsoft.aspnetcore.authentication.jwtbearer\8.0.6

- microsoft.aspnetcore.diagnostics.entityframeworkcore\8.0.6

- microsoft.aspnetcore.diagnostics\2.2.0

- microsoft.aspnetcore.identity.entityframeworkcore\8.0.6

- microsoft.entityframeworkcore.design\8.0.6

- microsoft.entityframeworkcore.sqlserver\8.0.6

- microsoft.entityframeworkcore\8.0.6

- microsoft.openapi\1.6.15

- swashbuckle.aspnetcore.swaggergen\6.6.2

- swashbuckle.aspnetcore.swaggerui\6.6.2