using Focus.Application.Services;
using Focus.Infra;
using Microsoft.EntityFrameworkCore;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ActivityRepository>();
builder.Services.AddScoped<GroupRepository>();
builder.Services.AddScoped<UserGroupRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<UserGroupService>();
builder.Services.AddScoped<FeedService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<FocusDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
