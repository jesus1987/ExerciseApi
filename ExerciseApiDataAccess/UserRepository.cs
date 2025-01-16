
using ExerciseApiDataAccess.Entities;
using ExerciseApiDataAccess.Map;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExerciseApiDataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = string.Format(configuration.GetConnectionString("DefaultConnection"), "ExerciseApidb");
        }

        public async Task<int> CreateAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Users (Name, LastName, Email, Password) 
            VALUES (@Name, @LastName, @Email, @Password);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    connection.Open();
                    return (int)(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, LastName, Email FROM Users";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(SqlMapper.MapToObject<User>(reader));
                        }
                    }
                }
            }

            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, LastName, Email FROM Users WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    using (var reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            return SqlMapper.MapToObject<User>(reader);
                        }
                    }
                }
            }
            return null; 
        }

        public async Task<User> UpdateAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Update query
                string updateQuery = "UPDATE Users SET Name, LastName, Email WHERE Id = @Id";

                // Select query to fetch the updated record
                string selectQuery = "SELECT Id, Name, LastName, Email FROM Users WHERE Id = @Id";

                using (var command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    await connection.OpenAsync();
                    // Execute the update command
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    // If no rows were updated, return null
                    if (rowsAffected == 0)
                    {
                        return null;
                    }

                    // Retrieve the updated object
                    command.CommandText = selectQuery;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return SqlMapper.MapToObject<User>(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Users WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<List<User>> GetUserByNaturalQuery(string query)
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(SqlMapper.MapToObject<User>(reader));
                        }
                    }
                }
            }

            return users;
        }

        public async Task<User> GetByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, LastName, Email, Password FROM Users WHERE Email = @email";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    await connection.OpenAsync();
                    using (var reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            return SqlMapper.MapToObject<User>(reader);
                        }
                    }
                }
            }
            return null;
        }
    }
}
