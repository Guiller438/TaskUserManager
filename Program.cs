using Microsoft.EntityFrameworkCore;
using System;
using TaskUserManager.Data;
using TaskUserManager.Repositories;
using TaskUserManager.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITaskUserRepository, TaskUserRepository>();
builder.Services.AddScoped<ITaskUserService, TaskUsersService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HTTP Client for Image Uploading Service
builder.Services.AddHttpClient<FileUploadService>(client =>
{
    //client.BaseAddress = new Uri("http://localhost:7010");

    // SSL Certificate Configuration => Delete this on "Production"
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});


builder.Services.AddDbContext<DbAb0bdeTalentseedsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
