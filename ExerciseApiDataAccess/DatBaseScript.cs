namespace ExerciseApiDataAccess;
public static class DatBaseScript
{
    public const string DataBaseScript = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ExerciseApidb') CREATE DATABASE ExerciseApidb;";
    public const string CreateTableQuery = @$"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
        BEGIN
            {CreateUserTable}
        END;";

    public const string CreateUserTable = @"CREATE TABLE Users (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                LastName NVARCHAR(100) NOT NULL,
                Password NVARCHAR(100) NULL,
                Email NVARCHAR(100) NOT NULL,
                CreatedAt DATETIME DEFAULT GETDATE()
            );";
}