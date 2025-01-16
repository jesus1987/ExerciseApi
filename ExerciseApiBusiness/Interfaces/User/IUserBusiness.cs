using ExerciseApiViewModel.ViewModels.User;
using FluentResults;
namespace ExerciseApiBusiness.Interfaces.User;
public interface IUserBusiness
{
    Task<Result<List<UserViewModel>>> GetAllAsync();
    Task<Result<UserViewModel>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(UserViewModel userViewModel);
    Task<Result<UserViewModel>> UpdateAsync(UserViewModel userViewModel);
    Task<Result<bool>> DeleteAsync(int id);

    Task<Result<bool>> ValidateUserAsync(string email, string password);
}