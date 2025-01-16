using ExerciseApiBusiness.Helpers;
using ExerciseApiBusiness.Interfaces.User;
using ExerciseApiDataAccess;
using ExerciseApiViewModel.ViewModels.User;
using FluentResults;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ExerciseApiBusiness.Implementation.User;

public class UserBusiness : IUserBusiness
{
    private string Salt = "Exercise*(_/@";
    private readonly IUserRepository _userRepository;
    public UserBusiness(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<int>> CreateAsync(UserViewModel userViewModel)
    {
        var validationErrors = ValidationHelper.ValidateObject(userViewModel);
        if (validationErrors.Any())
        {
            return Result.Fail(validationErrors);
        }

        var entity = Mapper.MapProperties<UserViewModel, ExerciseApiDataAccess.Entities.User>(userViewModel);
        if (!string.IsNullOrEmpty(entity.Password)) 
        { 
            entity.Password = HashPassword(entity.Password);
        }

        var result = await _userRepository.CreateAsync(entity);
        return Result.Ok(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return Result.Fail("Invalid ID. ID must be greater than 0.");
        }
        var getUser = await _userRepository.GetByIdAsync(id);
        if (getUser == null)
        {
            return Result.Fail("User not found.");
        }

        var result = await _userRepository.DeleteAsync(id);
        return Result.Ok(result);
    }

    public async Task<Result<List<UserViewModel>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var response = new List<UserViewModel>();
        foreach (var user in users)
        {
            response.Add(Mapper.MapProperties<ExerciseApiDataAccess.Entities.User, UserViewModel>(user));
        }

        return Result.Ok(response);
    }

    public async Task<Result<UserViewModel>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return Result.Ok(Mapper.MapProperties<ExerciseApiDataAccess.Entities.User, UserViewModel>(user));
    }

    public async Task<Result<UserViewModel>> UpdateAsync(UserViewModel userViewModel)
    {
        if (userViewModel.Id <= 0)
        {
            return Result.Fail("Invalid ID. ID must be greater than 0.");
        }

        var entity = Mapper.MapProperties<UserViewModel, ExerciseApiDataAccess.Entities.User>(userViewModel);
        if (!string.IsNullOrEmpty(entity.Password))
        {
            entity.Password = HashPassword(entity.Password);
        }
        var result = await _userRepository.UpdateAsync(entity);
        return Result.Ok(Mapper.MapProperties<ExerciseApiDataAccess.Entities.User, UserViewModel>(result));
    }

    public async Task<Result<bool>> ValidateUserAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Fail("Email cannot be empty or null.");
        }

        if (string.IsNullOrEmpty(password))
        {
            return Result.Fail("Password cannot be empty or null.");
        }

        var result = await _userRepository.GetByEmail(email);
        if (result is null)
        {
            return Result.Fail("Interanal error");
        }

        return result.Password == HashPassword(password) ? Result.Ok(true) : Result.Fail("Interanal error.");
    }

    private string HashPassword(string password)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Salt));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}