using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;


var builder = WebApplication.CreateBuilder(args);

// SqLite   db connection 
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

var app = builder.Build();

// Db creation in first run
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<TaskDbContext>();

    db.Database.EnsureCreated();

/*test to add some tasks to the db in first run


if (!db.Tasks.Any())
{
    db.Tasks.Add(new TaskManagerAPI.Models.TodoItem
    {
        Title = "test",
        IsCompleted = false
    });

    db.Tasks.Add(new TaskManagerAPI.Models.TodoItem
    {
        Title = "api test",
        IsCompleted = true
    });

    db.SaveChanges();
}
*/
}

//calls all tasks fro mthe db "lädt"
app.MapGet("/tasks", async (TaskDbContext db) =>
{
    return await db.Tasks.ToListAsync();
});


// creates new task in the db
app.MapPost("/tasks", async (
    TaskManagerAPI.Models.TodoItem task,
    TaskDbContext db) =>
{
    db.Tasks.Add(task);

    await db.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
});

// edit existing tasks 
app.MapPut("/tasks/{id}", async (
    int id,
    TaskManagerAPI.Models.TodoItem updatedTask,
    TaskDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);

    if (task == null)
    {
        return Results.NotFound();
    }

    task.Title = updatedTask.Title;
    task.IsCompleted = updatedTask.IsCompleted;

    await db.SaveChangesAsync();

    return Results.Ok(task);
});


// delete Task
app.MapDelete("/tasks/{id}", async (
    int id,
    TaskDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);

    if (task == null)
    {
        return Results.NotFound();
    }

    db.Tasks.Remove(task);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();


