using BasicBoilerplate;
using BasicBoilerplate.Interfaces;
using BasicBoilerplate.Models.Database;
using BasicBoilerplate.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region DBContext

builder.Configuration.AddJsonFile("appsettings.json");

var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");

#endregion

#region Swagger


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Logpress API", Version = "v1" });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

#endregion

#region DependecyInjection

builder.Services.AddScoped(typeof(IRepositoryService<>), typeof(GeneralService<>));
builder.Services.AddScoped<IGeneralService<User>, UserService>();
builder.Services.AddScoped<IUserService, AuthService>();
builder.Services.AddMemoryCache();

#endregion

#region JWTToken

// JWT token authentication

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var appSettings = appSettingsSection.Get<AppSettings>();

builder.Services.Configure<AppSettings>(appSettingsSection);

var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
#endregion

///TODO FluentValidation eklenebilir


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Logpress API");
    });
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
