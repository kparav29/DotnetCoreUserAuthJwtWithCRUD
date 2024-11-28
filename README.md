Prerequisite:
.Net Core 6
.Net Core Entity Framework 6
SqlServer
-----------------------------------------------------------------------------------------------------------
After Cloning the project to create databae on local environment go to appettings.json file and enter connection string.
Go to Package manager Console and run the command to create database - 
dotnet ef migrations add InitialDatabaseCreate
dotnet ef database update
-----------------------------------------------------------------------------------------------------
Run the project and open Swagger UI 
First create admin user - using endpoint register-admin, then login with the same user once login successfull in result Jwt token will be provided which is used to auhorize then http request, To do so Click Authorize button on swagger UI and enter Jwt token at given format.

-------------------------------------------


