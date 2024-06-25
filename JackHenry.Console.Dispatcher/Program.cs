using JackHenry.Console;
using JackHenry.Console.Dispatcher;
using JackHenry.Console.Dispatcher.Interfaces;
using JackHenry.MessageBroker.IoC;
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
				.AddMessageBroker(config)
				.AddSingleton<ICommandDispatcher, CommandDispatcher>());
	}

	public static async Task Main(string[] args)
	{
		var dispatcher = ServiceProvider.GetService<ICommandDispatcher>();

		await dispatcher.DispatchAsync();
	}
}