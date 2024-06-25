using JackHenry.Console.Interfaces;
using JackHenry.Settings.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JackHenry.Console;

public abstract class ProgramBase
{
	protected static IConfiguration BuildConfig() => new ConfigurationBuilder().BuildConfig();

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