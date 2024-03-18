# ASP.NET Inventory MySql

.NET using MySQL for database to interact user with inventory items

## How to Run?

> Migrate first the data into databse.

- Create migration name
```
Add-Migration InitialCreate
```

- Push migration that has been created
```
Update-Database
```

- If there's any problem with migration, try to delete first
```
Remove-Migration
```