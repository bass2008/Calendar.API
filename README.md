# Calendar.API

1) Ensure .net Core 2.2, PostgreSQL, PgAdmin, Git was installed

2) Go to the folder where the repository will be located, for example, "C:\Git".
Clone repository in your directory and switch branch on "dev":

### `cd C:\git`
### `git clone https://github.com/bass2008/Calendar.API.git`
### `cd Calendar.API`
### `git checkout dev`

3) If you want to use local database run next steps. You could use database from development environment and skip them.

### Skip if you use development database
### `cd C:\Git\Calendar.API\Calendar.API`
### `dotnet tool install --global dotnet-ef --version 2.2.6`
### `dotnet-ef database update`

4) Run project with debug mode for local development:
### `dotnet run` 
or run project with release mode for development/stage/production environment:
### `dotnet run -c Release`

5) Project is found on: 
http://localhost:5200/graphiql/


Examples of GraphQL queries:

### `GraphQL requests:`

### `1. Login`

mutation login($loginData: LoginInput!){
  login(loginData: $loginData) {expiresAt, token, user{id, email} }
}

Query variables:
{
  "loginData": {
    "email": "test@test.com",
    "password": "222"
  }
}

### `2. SignUp`

mutation signUp($signUpData: SignUpInput! ){
  signUp(signUpData: $signUpData)
}

Query variables:
{
  "signUpData": {
    "email": "test",
    "password": "123"
  }
}

### `3. Get tabs`

query Tabs{
  tabs{id, name, logo, events{title}}
}

### `4. Add Event`

mutation AddEvent($createData: EventInput!){
  createEvent(createData: $createData)
}

Query variables:
{
  "createData": {
    "title": "test event title",
    "description": "test event desc",
    "start": 1,
    "end": 2,
    "repeat": 1,
    "notification": 1,
    "tabId": 1
  }
}

### `5. Add Tab`
mutation AddTab($createData: TabInput!){
  createTab(createData: $createData)
}

Query variables:
{
  "createData": {
    "name": "test tab 2",
    "logo": "test logo 2"    
  }
}













