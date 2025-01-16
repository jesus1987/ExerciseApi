
using ExerciseApiBusiness.Implementation.User;
using ExerciseApiDataAccess;
using ExerciseApiViewModel.ViewModels.User;
using Moq;
using System.Reflection;

namespace ExerciseApiTest.IntegrationTest;

[TestClass]
public class UserBusinessTests
{
    [TestMethod]
    public async Task CreateAsync_ShouldReturnSuccess_WhenValidUserViewModel()
    {
        // Arrange
        var userViewModel = new UserViewModel
        {
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var userEntity = new ExerciseApiDataAccess.Entities.User
        {
            Name = userViewModel.Name,
            LastName = userViewModel.LastName,
            Email = userViewModel.Email
        };


        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<ExerciseApiDataAccess.Entities.User>()))
                      .ReturnsAsync(1);

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.CreateAsync(userViewModel);

        // Assert
        Assert.IsTrue(result.IsSuccess, "Result should indicate success.");
        Assert.AreEqual(1, result.Value);

        mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<ExerciseApiDataAccess.Entities.User>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var userViewModel = new UserViewModel
        {
            Name = "",
            LastName = "",
            Email = "valid-email"
        };

        var validationErrors = new List<string> { "Name is required.", "LastName is required." };

        var mockRepository = new Mock<IUserRepository>();


        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.CreateAsync(userViewModel);

        // Assert
        Assert.IsFalse(result.IsSuccess, "Result should indicate failure.");
        CollectionAssert.AreEqual(validationErrors, result.Reasons.Select(row=>row.Message).ToList());
    }

    [TestMethod] 
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var mockUsers = new List<ExerciseApiDataAccess.Entities.User>
        {
            new ExerciseApiDataAccess.Entities.User { Id = 1, Name = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new ExerciseApiDataAccess.Entities.User { Id = 2, Name = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        };

        var expectedUsers = new List<UserViewModel>
        {
            new UserViewModel { Id = 1, Name = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new UserViewModel { Id = 2, Name = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        };

        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(mockUsers);

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.IsTrue(result.IsSuccess, "Result should indicate success.");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(expectedUsers.Count, result.Value.Count);
        CollectionAssert.AreEqual(expectedUsers.Select(u => u.Id).ToList(), result.Value.Select(u => u.Id).ToList());
        CollectionAssert.AreEqual(expectedUsers.Select(u => u.Email).ToList(), result.Value.Select(u => u.Email).ToList());

        mockRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var mockUser = new ExerciseApiDataAccess.Entities.User
        {
            Id = userId,
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var expectedUser = new UserViewModel
        {
            Id = userId,
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.GetByIdAsync(userId);

        // Assert
        Assert.IsTrue(result.IsSuccess, "Result should indicate success.");
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(expectedUser.Id, result.Value.Id);
        Assert.AreEqual(expectedUser.Email, result.Value.Email);

        mockRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    }

    [TestMethod] // or [TestMethod] for MSTest
    public async Task UpdateAsync_ShouldReturnUpdatedUser_WhenValidInput()
    {
        // Arrange
        var userViewModel = new UserViewModel
        {
            Id = 1,
            Name = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "updated.email@example.com",
        };

        var updatedUserEntity = new ExerciseApiDataAccess.Entities.User
        {
            Id = 1,
            Name = "CreateFirstName",
            LastName = "CreateLastName",
            Email = "create.email@example.com"
        };

        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ExerciseApiDataAccess.Entities.User>()))
                      .ReturnsAsync(updatedUserEntity);

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.UpdateAsync(userViewModel);

        mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ExerciseApiDataAccess.Entities.User>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldReturnFailure_WhenInvalidId()
    {
        // Arrange
        var userViewModel = new UserViewModel
        {
            Id = 0,
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123!"
        };

        var mockRepository = new Mock<IUserRepository>();

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.UpdateAsync(userViewModel);

        // Assert
        Assert.IsFalse(result.IsSuccess, "Result should indicate failure.");
        Assert.AreEqual("Invalid ID. ID must be greater than 0.", result.Reasons.Select(row =>row.Message).FirstOrDefault());

        mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ExerciseApiDataAccess.Entities.User>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        var userViewModel = new UserViewModel
        {
            Id = 1,
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123!"
        };

        var userEntity = new ExerciseApiDataAccess.Entities.User
        {
            Id = 1,
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123!"
        };

        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ExerciseApiDataAccess.Entities.User>()))
                      .ThrowsAsync(new Exception("Repository error"));

     

        var service = new UserBusiness(mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(async () => await service.UpdateAsync(userViewModel));

        mockRepository.Verify(repo => repo.UpdateAsync(It.Is<ExerciseApiDataAccess.Entities.User>(u =>
            u.Id == userEntity.Id &&
            u.Email == userEntity.Email
        )), Times.Once);

    }

    [TestMethod]
    public async Task ValidateUserAsync_ShouldReturnTrue_WhenEmailAndPasswordAreValid()
    {
        // Arrange
        var email = "user@example.com";
        var password = "SecurePassword123!";
        var hashedPassword = "d/76I/3U0X8LDd2MHt9Zk15F3stovRU1B1yz2K5zUnU="; 

        var userEntity = new ExerciseApiDataAccess.Entities.User
        {
            Email = email,
            Password = hashedPassword
        };

        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetByEmail(email))
                      .ReturnsAsync(userEntity);

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.ValidateUserAsync(email, password);

        // Assert
        Assert.IsTrue(result.IsSuccess, "Result should indicate success.");
        Assert.IsTrue(result.Value, "Result value should be true.");
        mockRepository.Verify(repo => repo.GetByEmail(email), Times.Once);
    }

    [TestMethod]
    public async Task ValidateUserAsync_ShouldReturnFailure_WhenEmailIsEmpty()
    {
        // Arrange
        var email = string.Empty;
        var password = "SecurePassword123!";

        var mockRepository = new Mock<IUserRepository>();
        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.ValidateUserAsync(email, password);

        // Assert
        Assert.IsFalse(result.IsSuccess, "Result should indicate failure.");
        Assert.AreEqual("Email cannot be empty or null.", result.Reasons.Select(row=>row.Message).FirstOrDefault());
        mockRepository.Verify(repo => repo.GetByEmail(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task ValidateUserAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "SecurePassword123!";

        var mockRepository = new Mock<IUserRepository>();
        mockRepository.Setup(repo => repo.GetByEmail(email)).ReturnsAsync((ExerciseApiDataAccess.Entities.User)null);

        var service = new UserBusiness(mockRepository.Object);

        // Act
        var result = await service.ValidateUserAsync(email, password);

        // Assert
        Assert.IsFalse(result.IsSuccess, "Result should indicate failure.");
        Assert.AreEqual("Interanal error", result.Reasons.Select(row => row.Message).FirstOrDefault());
        mockRepository.Verify(repo => repo.GetByEmail(email), Times.Once);
    }

}
