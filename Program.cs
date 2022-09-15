using Microsoft.EntityFrameworkCore;
using MinimalAPI_Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("Todos"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync().ConfigureAwait(false);

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync().ConfigureAwait(false));

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(x => x.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id).ConfigureAwait(false) is Todo todo ? Results.Ok(todo) : Results.NotFound());

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id).ConfigureAwait(false);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync().ConfigureAwait(false);

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id).ConfigureAwait(false);

    if (todo is null) return Results.NotFound();
    
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();

    return Results.Ok(todo);
});

app.Run();