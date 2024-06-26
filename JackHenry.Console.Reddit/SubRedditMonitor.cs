using JackHenry.Console.Interfaces;
using JackHenry.Console.Reddit.Extensions;
using JackHenry.Console.Reddit.Interfaces;
using JackHenry.Proxy.CRUD.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Console.Reddit;

/// <inheritdoc />
internal class SubRedditMonitor : ISubRedditMonitor
{
	public SubRedditMonitor(
		ICrudProxy proxy,
		IDisplay display,
		ISubRedditCollection subReddits)
	{
		ArgumentNullException.ThrowIfNull(proxy);
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(subReddits);

		Proxy = proxy;
		Display = display;
		SubReddits = subReddits;
	}

	private ICrudProxy Proxy { get; }
	private IDisplay Display { get; }
	private ISubRedditCollection SubReddits { get; }

	private EventWaitHandle QueryReady { get; } = new EventWaitHandle(false, EventResetMode.AutoReset);
	private ConcurrentQueue<string> QueryQueue { get; } = new ConcurrentQueue<string>();

	/// <inheritdoc />
	public async Task MonitorAsync()
	{
		Display.Start();

		var tasks =
			new[]
			{
				SubReddits.StartListenerAsync(),
				DispatchAsync(),
				QueryRedditAsync(),
			};

		await Task.WhenAll(tasks);
	}

	private async Task DispatchAsync()
	{
		while (true)
		{
			await Task.Delay(1000);

			foreach (var subReddit in SubReddits)
			{
				var name = subReddit?.Name;

				if (string.IsNullOrEmpty(name))
					continue;

				QueryQueue.Enqueue(name);

				QueryReady.Set();
			}
		}
	}

	private async Task QueryRedditAsync()
	{
		while (true)
		{
			QueryReady.WaitOne();

			while (QueryQueue.TryDequeue(out var name))
			{
				var subReddit = SubReddits[name];

				if (subReddit is null)
					continue;

				Display.WriteLine($"Quering Reddit for subreddit: r/{name}");
			}
		}
	}
}