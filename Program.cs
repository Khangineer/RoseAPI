using Microsoft.EntityFrameworkCore;
using RoseAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:7202")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });

});

builder.Services.AddDbContext<RoseDBContext>(options =>
{
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=RoseDB;Trusted_Connection=True;");
    options.EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("CORSPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
