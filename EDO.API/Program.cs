using EDO.Access;
using EDO.Access.Models;
using EDO.API;
using EDO.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["AccessConfiguration:Audience"],
        ValidIssuer = configuration["AccessConfiguration:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Vars.TheSecretKey))
    };
});

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseSwagger();
app.UseSwaggerUI();

await app.UseRoleInitializerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
