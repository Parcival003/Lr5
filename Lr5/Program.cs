using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("index.html"));
});

app.MapPost("/submit", async context =>
{
    var value = context.Request.Form["value"];
    var expiration = context.Request.Form["expiration"];

    if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(expiration))
    {
        if (DateTime.TryParse(expiration, out DateTime expirationDate))
        {
            context.Response.Cookies.Append("storedValue", value, new CookieOptions
            {
                Expires = expirationDate,
                HttpOnly = true
            });
            await context.Response.WriteAsync("Data saved in Cookies!");
        }
        else
        {
            await context.Response.WriteAsync("Invalid expiration date and time format.");
        }
    }
    else
    {
        await context.Response.WriteAsync("Please enter a value and expiration date and time.");
    }
});

app.MapGet("/check", async context =>
{
    var storedValue = context.Request.Cookies["storedValue"];
    if (!string.IsNullOrEmpty(storedValue))
    {
        await context.Response.WriteAsync($"Value in Cookies: {storedValue}");
    }
    else
    {
        await context.Response.WriteAsync("No data saved in Cookies.");
    }
});
app.UseMiddleware<ErrorLoggingMiddleware>();
app.Run();
