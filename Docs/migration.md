dotnet ef migrations add InitialCreate --project src/infrastructure --startup-project src/api --output-dir Persistence/Migrations

dotnet ef migrations add IdentityFoundation --project src/infrastructure --startup-project src/api
dotnet ef database update --project src/infrastructure --startup-project src/api
dotnet ef migrations list --project src/infrastructure --startup-project src/api