using WordSearch.Application.Interfaces;
using WordSearch.Application.Services;
using WordSearch.Infrastructure.Interfaces;
using WordSearch.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// Register repositories and services in the dependency container
builder.Services.AddSingleton<IMatrixRepository, InMemoryMatrixRepository>(); // In-memory repository
builder.Services.AddScoped<IWordSearchService, WordSearchService>();          // Word search service

// Add controller and Swagger services for documentation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request handling pipeline
if (app.Environment.IsDevelopment())
{
    // Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Enforce HTTPS redirection

app.UseAuthorization(); // Authorization middleware (if needed in the future)

app.MapControllers(); // Map registered controllers in the container

app.Run(); // Run the application
