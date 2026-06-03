/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/
using AiImageTaskManager.Infrastructure.FileStorage;

using AiImageTaskManager.Infrastructure.BackgroundJobs;

using AiImageTaskManager.Application.Interfaces;
using AiImageTaskManager.Infrastructure.Services;

using AiImageTaskManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IImageTaskService, ImageTaskService>();
builder.Services.AddScoped<IImageFileStorageService, LocalImageFileStorageService>();
builder.Services.AddHttpClient<IApiTestCaseService, ApiTestCaseService>();

//builder.Services.AddHostedService<ImageTaskBackgroundService>();
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddHostedService<ImageTaskBackgroundService>();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }