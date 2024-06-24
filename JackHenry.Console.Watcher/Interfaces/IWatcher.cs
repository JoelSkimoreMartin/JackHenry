using System.Threading.Tasks;

namespace JackHenry.Console.Watcher.Interfaces;

public interface IWatcher
{
	Task StartAsync();

	Task WatchAsync();
}