using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register API services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure Swagger and HTTPS redirection
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define the POST endpoint to send an email
app.MapPost("/sendEmail", (Business business) =>
{
    if (business == null)
    {
        return Results.BadRequest("Business data is missing.");
    }
    return Results.Ok();
});

app.MapControllers();
app.Run();
