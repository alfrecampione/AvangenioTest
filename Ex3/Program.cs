using System.Security.Claims;
using System.Text;
using Ex3.Data.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ex3.DataAccess;
using Ex3.DataAccess.Repositories;
using InitialData;
using Ex3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add database service.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") + ";TrustServerCertificate=true";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456")), // Replace with your secret key
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "GSI Challenge Authenticator",
            ValidAudience = "www.gsichanllengeapi.com",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            // RoleClaimType = ClaimTypes.Country,
            // Specify the algorithm
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var user = context.Principal;

                // Check if the token has the 'Country' claim
                if (!user.HasClaim(c => c.Type == ClaimTypes.Country))
                {
                    // Reject the token
                    context.Fail("JWT does not contain 'Country' claim");
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 5;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

// builder.Services.AddTransient<IdentityUser, User>();
builder.Services.AddTransient<IDataRepository, DataRepository>();
builder.Services.AddTransient<IContactService, ContactService>();

// TODO: Add more services here.

// Add controller services to build the api.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Takes only one of the controllers in the same route in case of conflict.
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

//CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
//-------------


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<User>>();

    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    // Applies any pending migrations for the context to the database
    dbContext.Database.Migrate();

    var dbInitializer = new DatabaseInitializer(dbContext, roleManager, userManager);
    dbInitializer.EnsureInitialData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAllOrigins");

//app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallback(context =>
{
    context.Response.StatusCode = 404;
    return context.Response.WriteAsync("Invalid URL");
});

app.Run();