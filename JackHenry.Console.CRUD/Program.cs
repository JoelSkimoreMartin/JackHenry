using JackHenry.Console;
using JackHenry.Console.CRUD;
using JackHenry.Console.CRUD.Interfaces;
using JackHenry.MessageBroker.IoC;
using JackHenry.Proxy.CRUD.IoC;
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
				.AddMessageBroker(config)
				.AddSingleton<ICrudManager, CrudManager>());
	}

	public static async Task Main(string[] args)
	{
		var manager = ServiceProvider.GetService<ICrudManager>();

		await manager.ManageAsync();
	}
}