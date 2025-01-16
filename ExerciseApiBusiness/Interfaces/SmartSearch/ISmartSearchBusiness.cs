
using ExerciseApiViewModel.ViewModels.User;
using FluentResults;

namespace ExerciseApiBusiness.Interfaces.SmartSearch;

public interface ISmartSearchBusiness
{
    Task<Result<List<UserViewModel>>> SearchAsync(string naturalQuery);
}
