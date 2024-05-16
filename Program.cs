using System.Text;
using Event_planning_back.Application.Services;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Security;
using Event_planning_back.DataAccess;
using Event_planning_back.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration.GetSection("Jwt:SecretKey").Get<string>();
var LocalhostCors = "_localhostCors";

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(LocalhostCors, builder =>
       builder.SetIsOriginAllowed(origin => new Uri(origin).Host.EndsWith("localhost"))
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventPlanningDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(EventPlanningDbContext)));
    });

builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));


builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    if (jwtKey != null)
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AuthToken"];

            return Task.CompletedTask;
        }
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(LocalhostCors);
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
