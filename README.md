# Calendar.API

1) Ensure .net Core 2.2, PostgreSQL, PgAdmin, Git was installed

2) Go to the folder where the repository will be located, for example, "C:\Git".
Clone repository in your directory and switch branch on "dev":

### `cd C:\git`
### `git clone <URL>`
### `cd Spartan.API`
### `git checkout dev`

3) If you want to use local database run next steps. You could use database from development environment and skip them.

### Skip if you use development database
### `cd C:\Git\Spartan.API\Spartan.API`
### `dotnet tool install --global dotnet-ef --version 2.2.6`
### `dotnet-ef database update`

4) Run project with debug mode for local development:
### `dotnet run` 
or run project with release mode for development/stage/production environment:
### `dotnet run -c Release`

5) Project is found on: 
http://localhost:5200/graphiql/

