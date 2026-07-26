## add project
dotnet sln church-website.slnx add src/api/api.csproj
---
## add reference

## add classlib
dotnet new classlib -n infrastructure
---
## to check if all project is added to ur sln
dotnet sln church-website.slnx list
## to esc merge conflit cli
:q!
---