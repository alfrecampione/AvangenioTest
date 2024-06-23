Steps for run each exercice:

- `cd Ex<exercice_number>`
    
- `dotnet run`

Additionally when running the Ex3 you need to run `dotnet restore` after `cd Ex3` so you can install all de NuGet dependecies

If any changes are applied to the models just run the following commands: 

`dotnet ef migrations add <MigrationName>`

`dotnet ef database update`