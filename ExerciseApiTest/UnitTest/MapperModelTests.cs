
using ExerciseApiDataAccess.Entities;
using ExerciseApiViewModel.ViewModels.User;

namespace ExerciseApiTest.UnitTest;

[TestClass]
public class MapperModelTests
{
    [TestMethod]
    public void MapProperties_ValidSourceEntityAndTargetViewModel_ReturnsMappedTarget()
    {
        // Arrange
        var source = new User
        {
            Email = "email Test",
            LastName = "Last Name Test",
            Name = "Test Name",
            Id = 1,
            Password = "Password",
            CreatedAt = new DateTime(2023, 1, 1)
        };

        // Act
        var result = ExerciseApiBusiness.Helpers.Mapper.MapProperties<User, UserViewModel>(source);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(source.Id, result.Id);
        Assert.AreEqual(source.Name, result.Name);
        Assert.AreEqual(source.LastName, result.LastName);
        Assert.AreEqual(source.Email, result.Email);
        Assert.AreEqual(source.CreatedAt, result.CreatedAt);
    }

    [TestMethod]
    public void MapProperties_ValidSourceViewModelAndTargetEntity_ReturnsMappedTarget()
    {
        // Arrange
        var source = new UserViewModel
        {
            Email ="email Test",
            LastName = "Last Name Test",
            Name = "Test Name",
            Id = 1,
            Password = "Password",
            CreatedAt = new DateTime(2023, 1, 1)
        };

        // Act
        var result = ExerciseApiBusiness.Helpers.Mapper.MapProperties<UserViewModel, User>(source);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(source.Id, result.Id);
        Assert.AreEqual(source.Name, result.Name);
        Assert.AreEqual(source.LastName, result.LastName);
        Assert.AreEqual(source.Email, result.Email);
        Assert.AreEqual(source.CreatedAt, result.CreatedAt);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void MapProperties_NullSource_ThrowsArgumentNullException()
    {
        // Act
        ExerciseApiBusiness.Helpers.Mapper.MapProperties<UserViewModel, User>(null);
    }
}
