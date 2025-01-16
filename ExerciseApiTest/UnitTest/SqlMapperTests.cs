
using ExerciseApiDataAccess.Entities;
using ExerciseApiDataAccess.Map;
using Microsoft.Data.SqlClient;
using Moq;
using System.Data;

namespace ExerciseApiTest.UnitTest;

[TestClass]
public class SqlMapperTests
{
    [TestMethod]
    public void MapToObject_ValidSqlDataReader_ReturnsMappedObject()
    {
        var mockReader = new Mock<IDataReader>();

        mockReader.Setup(r => r.FieldCount).Returns(5);
        mockReader.Setup(r => r.GetName(0)).Returns("Id");
        mockReader.Setup(r => r.GetName(1)).Returns("Name");
        mockReader.Setup(r => r.GetName(2)).Returns("LastName");
        mockReader.Setup(r => r.GetName(3)).Returns("Email");
        mockReader.Setup(r => r.GetName(4)).Returns("CreatedAt");

        mockReader.Setup(r => r["Id"]).Returns(1);
        mockReader.Setup(r => r["Name"]).Returns("Test Name");
        mockReader.Setup(r => r["LastName"]).Returns("Last Test Name");
        mockReader.Setup(r => r["Email"]).Returns("Email address");
        mockReader.Setup(r => r["CreatedAt"]).Returns(new DateTime(2023, 1, 1));

        // Act
        var result = SqlMapper.MapToObject<User>(mockReader.Object);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("Test Name", result.Name);
        Assert.AreEqual("Last Test Name", result.LastName);
        Assert.AreEqual("Email address", result.Email);
        Assert.AreEqual(new DateTime(2023, 1, 1).Date, result.CreatedAt.Date);

    }
}
