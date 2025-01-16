

using System.ComponentModel.DataAnnotations;

namespace ExerciseApiBusiness.Helpers;
public class ValidationHelper
{
    public static List<string> ValidateObject(object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(obj);

        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        var errors = new List<string>();
        if (!isValid)
        {
            foreach (var validationResult in validationResults)
            {
                errors.Add(validationResult.ErrorMessage);
            }
        }

        return errors;
    }
}
