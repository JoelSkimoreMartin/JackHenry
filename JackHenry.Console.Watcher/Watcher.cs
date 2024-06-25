using JackHenry.Console.Interfaces;
using JackHenry.Console.Watcher.ExtensionMethods;
using JackHenry.Console.Watcher.Interfaces;
using JackHenry.Proxy.CRUD.Interfaces;
using System;
using System.Threading.Tasks;

namespace JackHenry.Console.Watcher;

/// <inheritdoc />
public class Watcher : IWatcher
{
	public Watcher(IDisplay display, ICrudProxy proxy, ISignalRClient signalR)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(proxy);
		ArgumentNullException.ThrowIfNull(signalR);

		Proxy = proxy;
		SignalR = signalR;
		Display = display;
	}

	private ICrudProxy Proxy { get; }
	private ISignalRClient SignalR { get; }
	private IDisplay Display { get; }

	/// <inheritdoc />
	public async Task StartAsync()
	{
		Display.Starting();

		await SignalR.StartAsync();
	}

	/// <inheritdoc />
	public async Task WatchAsync()
	{
		var subReddits = await Proxy.GetSubRedditsAsync();

		Display.Update(subReddits);

		SignalR.Signal.WaitOne();
	}
}