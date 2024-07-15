using Autofac;
using Autofac.Extensions.DependencyInjection;
using MtsTest.Interfaces;
using MtsTest.Services;
using Microsoft.Extensions.DependencyInjection;
using MtsTest.Models;
using Microsoft.EntityFrameworkCore;
using MtsTest.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configure Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<HttpBinService>().As<IHttpBinService>().InstancePerLifetimeScope();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
