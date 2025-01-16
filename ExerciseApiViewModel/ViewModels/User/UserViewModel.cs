
using System.ComponentModel.DataAnnotations;

namespace ExerciseApiViewModel.ViewModels.User;
public class UserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "LastName is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    public string Password { get; set; }

    public DateTime CreatedAt { get; set; }
}