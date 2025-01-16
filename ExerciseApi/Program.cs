

using ExerciseApi;
using ExerciseApiBusiness.Implementation.SmartSearch;
using ExerciseApiBusiness.Implementation.User;
using ExerciseApiBusiness.Interfaces.SmartSearch;
using ExerciseApiBusiness.Interfaces.User;
using ExerciseApiDataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserBusiness, UserBusiness>();
builder.Services.AddTransient<ISmartSearchBusiness, SmartSearchBusiness>();
builder.Services.AddSingleton<SqlInitializer>();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.Cookie.Name = "AppCookie";
    });
builder.Services.AddAuthorization();

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exercise Api V1");
        c.RoutePrefix = string.Empty; // Swagger UI available at root
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var sqlInitializer = app.Services.GetRequiredService<SqlInitializer>();
sqlInitializer.ExecuteSqlScript();
app.Run();

