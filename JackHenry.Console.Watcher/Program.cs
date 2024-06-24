using JackHenry.Console;
using JackHenry.Console.Watcher;
using JackHenry.Console.Watcher.Interfaces;
using JackHenry.Proxy.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public class Program : ProgramBase
{
	static Program()
	{
		var config = BuildConfig();

		SetUp(
			services =>
			services
				.AddWebApiProxy(config)
				.AddSingleton<IWatcher, Watcher>());
	}

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