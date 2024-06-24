using JackHenry.Console.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace JackHenry.Console;

public abstract class ProgramBase
{
	protected static IConfiguration BuildConfig() =>
			new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

	protected static void SetUp(Action<IServiceCollection> initializer)
	{
		ArgumentNullException.ThrowIfNull(initializer);

		var services = new ServiceCollection();

		services.AddScoped<IDisplay, Display>();

		initializer(services);

		ServiceProvider = services.BuildServiceProvider();
	}

	protected static ServiceProvider ServiceProvider { get; private set; }
}