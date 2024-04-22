using Abp.Domain.Repositories;
using ClassLibrary1;
using ClassLibrary2.Entidades;
using ClassLibrary2.Repositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEntityFrameworkNpgsql()
//                .AddDbContext<Contexto>(options => options
//                .UseNpgsql("Host=localhost;Port=5432;Pooling=true;Database=Teste1;Username=postgres;Password=06072003lmi;"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Contexto>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
