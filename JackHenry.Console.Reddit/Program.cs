using JackHenry.Console;
using JackHenry.Console.Reddit;
using JackHenry.Console.Reddit.Interfaces;
using JackHenry.MessageBroker.IoC;
using JackHenry.Proxy.CRUD.IoC;
using JackHenry.Proxy.Reddit.IoC;
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
				.AddCrudProxy(config)
				.AddRedditProxy(config)
				.AddMessageBroker(config)
				.AddScoped<ISubRedditMonitor, SubRedditMonitor>()
				.AddScoped<ISubRedditCollection, SubRedditCollection>());
	}

	public static async Task Main(string[] args)
	{
		var monitor = ServiceProvider.GetService<ISubRedditMonitor>();

		await monitor.MonitorAsync();
	}
}