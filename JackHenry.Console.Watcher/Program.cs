using JackHenry.Console.Watcher;
using JackHenry.Console.Watcher.Interfaces;
using JackHenry.Proxy.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
				.AddWebApiProxy(config)
				.AddSingleton<IDisplay, Display>()
				.AddSingleton<IWatcher, Watcher>()
				.BuildServiceProvider();
	}

	private static ServiceProvider ServiceProvider { get; }

	public static async Task Main(string[] args)
	{
		var watcher = ServiceProvider.GetService<IWatcher>();

		await watcher.StartAsync();

		while (true)
		{
			await watcher.WatchAsync();
		}
	}
}