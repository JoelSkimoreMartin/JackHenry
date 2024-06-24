using JackHenry.Console.Reddit.Interfaces;
using JackHenry.MessageBroker.IoC;
using JackHenry.Proxy.Reddit.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

public class Program
{
	static Program()
	{
		IConfiguration config =
			new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

		ServiceProvider =
			new ServiceCollection()
				.AddRedditProxy(config)
				.AddMessageBroker(config)
				.AddScoped<ISubRedditMonitor, ISubRedditMonitor>()
				.BuildServiceProvider();
	}

	private static ServiceProvider ServiceProvider { get; }

	public static async Task Main(string[] args)
	{
		var monitor = ServiceProvider.GetService<ISubRedditMonitor>();

		Console.WriteLine("Starting SignalR Client...");

		await monitor.StartAsync();

		while (true)
		{
			await monitor.MonitorAsync();
		}
	}
}