using JackHenry.Models;
using JackHenry.Repo.IoC;
using JackHenry.WebApi.CRUD.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services
	.AddRepository()
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(c =>
	{
		var assemblies =
			new[]
			{
				Assembly.GetExecutingAssembly(),
				typeof(SubReddit).Assembly,
			};

		foreach (var assembly in assemblies)
		{
			var assemblyName = assembly.GetName().Name;

			var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");

			c.IncludeXmlComments(xmlDocFile);
		}
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();

app.MapHub<CrudHub>($"/{Constants.SignalR.HubName}");

app.Run();