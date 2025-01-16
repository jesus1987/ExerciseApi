using ExerciseApiBusiness.Interfaces.SmartSearch;
using ExerciseApiDataAccess;
using ExerciseApiViewModel.ViewModels.User;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using FluentResults;
using ExerciseApiBusiness.Helpers;
using System.Net.Http.Headers;

namespace ExerciseApiBusiness.Implementation.SmartSearch;
public class SmartSearchBusiness : ISmartSearchBusiness
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public SmartSearchBusiness(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<Result<List<UserViewModel>>> SearchAsync(string naturalQuery)
    {
        if (string.IsNullOrEmpty(naturalQuery))
        {
            return Result.Fail<List<UserViewModel>>("naturalQuery cannot be empty");
        }

        var getSqlQuery = await GenerateSqlQuery(naturalQuery);
        if (string.IsNullOrEmpty(getSqlQuery)) 
        { 
            return Result.Fail<List<UserViewModel>>("Error generating SQL query");
        }

        var result = await _userRepository.GetUserByNaturalQuery(getSqlQuery);
        var response = new List<UserViewModel>();
        foreach (var user in result)
        {
            response.Add(Mapper.MapProperties<ExerciseApiDataAccess.Entities.User, UserViewModel>(user));
        }
        return response;
    }

    private async Task<string> GenerateSqlQuery(string naturalQuery)
    {
        string openAiApiKey = _configuration["openAiApiKey"];
        string apiEndpoint = _configuration["openAiApiUrl"];

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiApiKey);

        var requestBody = new
        {
            model = "gpt-4", 
            messages = new[]
            {
                new { role = "system", content = "You are a SQL expert." },
                new { role = "user", content = $"database table called Users(Id,Name,LastName, Email, CreatedAt, Password), Do NOT include any explanation or formatting such as code blocks, *Only return the SQL(version SQL Server 2022) query for*: {naturalQuery}" }
            },
            max_tokens = 500
        };

        string jsonBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(apiEndpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<JsonDocument>(responseBody);
            string sqlQuery = responseJson?.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return sqlQuery.Trim();
        }

        Console.WriteLine($"Error: {response.StatusCode}");
        return null;
    }
}