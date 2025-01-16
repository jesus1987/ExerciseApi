
using ExerciseApiDataAccess.Entities;

namespace ExerciseApiDataAccess;
public interface IUserRepository
{
    Task<int> CreateAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);

    Task<List<User>> GetUserByNaturalQuery(string query);

    Task<User> GetByEmail(string email);
}