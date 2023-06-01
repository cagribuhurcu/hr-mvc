using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using HRProject.Repositories.Concrete;
using HRProject.Repositories.Context;
using HRProject.Service.Abstract;
using HRProject.Service.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<HRProjectContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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
