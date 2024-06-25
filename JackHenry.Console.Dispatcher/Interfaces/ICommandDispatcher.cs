using System.Threading.Tasks;

namespace JackHenry.Console.Dispatcher.Interfaces;

internal interface ICommandDispatcher
{
	Task DispatchAsync();
}