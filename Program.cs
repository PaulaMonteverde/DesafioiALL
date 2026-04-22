using Microsoft.EntityFrameworkCore;
using Projeto_iALL.Data;
using Projeto_iALL.Services;
using Projeto_iALL.Services.Collaborator;
using Projeto_iALL.Services.Item;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<CollaboratorService>();
builder.Services.AddScoped<ItemService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
