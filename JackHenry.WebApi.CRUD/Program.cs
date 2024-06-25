using JackHenry.MessageBroker.IoC;
using JackHenry.Models;
using JackHenry.Repo.IoC;
using JackHenry.Settings.IoC;
using JackHenry.WebApi.CRUD.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

var config = new ConfigurationBuilder().BuildConfig();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services
	.AddRepository()
	.AddMessageBroker(config)
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