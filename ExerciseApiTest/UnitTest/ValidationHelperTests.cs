
using ExerciseApiBusiness.Helpers;
using ExerciseApiViewModel.ViewModels.User;

namespace ExerciseApiTest.UnitTest;

[TestClass]
public class ValidationHelperTests
{
    [TestMethod]
    public void ValidationHelper_ValidObject_ReturnsEmptyList()
    {
        // Arrange
        var source = new UserViewModel
        {
            Email = "email Test",
            LastName = "Last Name Test",
            Name = "Test Name",
            Id = 1,
            Password = "Password",
            CreatedAt = new DateTime(2023, 1, 1)
        };

        // Act
        var result = ValidationHelper.ValidateObject(source);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void ValidationHelper_InvalidObject_ReturnsErrorMessages()
    {
        // Arrange
        var userViewModel = new UserViewModel
        {
        };

        // Act
        var result = ValidationHelper.ValidateObject(userViewModel);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(result.Contains("Name is required."));
        Assert.IsTrue(result.Contains("LastName is required."));
        Assert.IsTrue(result.Contains("Email is required."));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ValidationHelper_NullObject_ThrowsArgumentNullException()
    {
        // Act
        ValidationHelper.ValidateObject(null);
    }
}