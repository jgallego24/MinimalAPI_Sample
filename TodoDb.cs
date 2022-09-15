using Microsoft.EntityFrameworkCore;

namespace MinimalAPI_Sample;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options)
    {
        
    }

    public DbSet<Todo> Todos => Set<Todo>();
}