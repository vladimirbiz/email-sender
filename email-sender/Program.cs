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

    // Email subject and body could be dynamically created based on the business data
    string subject = $"Hello from {business.Name}";
    string body = $"Dear {business.Name},\n\nThank you for contacting us. Your website is {business.Website}. We look forward to working with you.\n\nBest regards,\nYour Company";

    return Results.Ok();
});

app.MapControllers();
app.Run();
