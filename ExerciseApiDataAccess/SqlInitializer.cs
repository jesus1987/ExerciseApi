using ExerciseApiDataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExerciseApiDataAccess;
public class SqlInitializer
{
    private readonly IConfiguration _configuration;
    
    public SqlInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ExecuteSqlScript()
    {
        string _masterConnectionString = string.Format(_configuration.GetConnectionString("DefaultConnection"), "master");

        using (var connection = new SqlConnection(_masterConnectionString))
        {
            connection.Open();
            ExecuteQuery(connection, DatBaseScript.DataBaseScript);
        }

        string newDbConnectionString = _masterConnectionString.Replace("Database=master;", "Database=ExerciseApidb;");
        using (var connection = new SqlConnection(newDbConnectionString))
        {
            connection.Open();
            ExecuteQuery(connection, DatBaseScript.CreateTableQuery);
        }
    }

    private void ExecuteQuery(SqlConnection connection, string query)
    {
        using (var command = new SqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }
}