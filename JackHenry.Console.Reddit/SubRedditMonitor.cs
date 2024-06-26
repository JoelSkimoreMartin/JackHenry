using JackHenry.Console.Interfaces;
using JackHenry.Console.Reddit.Extensions;
using JackHenry.Console.Reddit.Interfaces;
using JackHenry.Proxy.CRUD.Interfaces;
using JackHenry.Proxy.Reddit.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Console.Reddit;

/// <inheritdoc />
internal class SubRedditMonitor : ISubRedditMonitor
{
	public SubRedditMonitor(
		ICrudProxy proxy,
		IRedditProxy redditProxy,
		IDisplay display,
		ISubRedditCollection subReddits)
	{
		ArgumentNullException.ThrowIfNull(proxy);
		ArgumentNullException.ThrowIfNull(redditProxy);
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(subReddits);

		Proxy = proxy;
		RedditProxy = redditProxy;
		Display = display;
		SubReddits = subReddits;
	}

	private ICrudProxy Proxy { get; }
	private IRedditProxy RedditProxy { get; }
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

			QueryReady.Set();
		}
	}

	private async Task QueryRedditAsync()
	{
		while (true)
		{
			QueryReady.WaitOne();

			while (true)
			{
				var names = new List<string>();

				foreach (var subReddit in SubReddits)
				{
					var name = subReddit?.Name;

					if (string.IsNullOrEmpty(name))
						continue;

					names.Add(name);
				}

				if (names.Any() == false)
					continue;

				Display.WriteLine($"Quering Reddit for subreddits: {string.Join(", ", names.Select(name => $"r/{name}"))}");

				var subReddits = await RedditProxy.QuerySubRedditAsync(names.ToArray());

				await SubReddits.UpdateAsync(subReddits);
			}
		}
	}
}