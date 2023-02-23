using EDO.Access;
using EDO.Access.Models;
using EDO.API;
using EDO.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration["ConnectionStrings:PostgresConnectionString"];
builder.Services.AddOptions();
builder.Services.Configure<AccessConfiguration>(configuration.GetSection("AccessConfiguration"));

builder.Services.AddDbContext<AccessDbContext>(option => option.UseNpgsql(connectionString));
builder.Services.AddDbContext<EdoDbContext>(option => option.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
{
    option.Password.RequiredLength = 8;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireLowercase = true;
    option.Password.RequireUppercase = false;
    option.Password.RequireDigit = true;
})
.AddRoles<ApplicationRole>()
.AddUserManager<UserManager<ApplicationUser>>()
.AddRoleManager<RoleManager<ApplicationRole>>()
.AddEntityFrameworkStores<AccessDbContext>()
.AddDefaultTokenProviders()
.AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseRoleInitializerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
