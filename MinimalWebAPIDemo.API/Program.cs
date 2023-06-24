using MinimalWebAPIDemo.API.Model;
using MinimalWebAPIDemo.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddSingleton<IBlogService, BlogService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



#region direct use endpoint on programe.cs file


//// Configure routes
app.MapGet("/api/blog", (IBlogService todoService) =>
{
    var todoItems = todoService.GetAll();
    return Results.Ok(todoItems);
});

app.MapGet("/api/blog/{id}", (IBlogService todoService, int id) =>
{
    var todoItem = todoService.GetById(id);
    if (todoItem == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(todoItem);
});

app.MapPost("/api/blog", async (IBlogService todoService, Blog todoItem) =>
{
    var createdTodoItem = todoService.Create(todoItem);
    return Results.Created($"/api/todo/{createdTodoItem.Id}", createdTodoItem);
});

app.MapPut("/api/blog/{id}", async (IBlogService todoService, int id, Blog updatedTodoItem) =>
{
    todoService.Update(id, updatedTodoItem);
    return Results.NoContent();
});

app.MapDelete("/api/blog/{id}", async (IBlogService todoService, int id) =>
{
    todoService.Delete(id);
    return Results.NoContent();
});

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
