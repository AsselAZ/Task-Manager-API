using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data;

// implements from Db context
//also said db connection 
public class TaskDbContext : DbContext
{
    //dbcontextOptions holds settings for db connection
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

//represents the tasts from the Db
    public DbSet<TodoItem> Tasks { get; set; }
}