using HeroesAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configBuilder = new ConfigurationBuilder();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();

configBuilder.AddJsonFile("appsettings.json");
IConfigurationRoot partialConfig = configBuilder.Build(); // build partial config
string connectionString = partialConfig["ConnectionStrings:DockerDB"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.WithOrigins("http://localhost:7019")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<HeroesContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
