//orginises the web app and sets up the endpoints for the apii
var builder = WebApplication.CreateBuilder(args);

//creates the web application
var app = builder.Build();

app.MapGet("/", () => "Task Manager API läuft!");


//api with 2 hard coded tasks
app.MapGet("/tasks", () =>
{
    return new[]
    {
        new { Id = 1, Title = "Sharp lernen", IsCompleted = false },
        new { Id = 2, Title = ".NET API", IsCompleted = false }
    };
});

app.Run();