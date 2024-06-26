using JackHenry.Console.Extensions;
using JackHenry.Console.Interfaces;
using JackHenry.Console.Reddit.Interfaces;
using JackHenry.MessageBroker.Commands;
using JackHenry.MessageBroker.Interfaces;
using JackHenry.Models;
using JackHenry.Proxy.CRUD.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackHenry.Console.Reddit;

/// <inheritdoc />
internal class SubRedditCollection : ISubRedditCollection
{
	public SubRedditCollection(
		ICrudProxy proxy,
		IDisplay display,
		ISubscriber<MonitorSubReddit> subscriber)
	{
		ArgumentNullException.ThrowIfNull(proxy);
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(subscriber);

		Proxy = proxy;
		Display = display;
		Subscriber = subscriber;
	}

	private ICrudProxy Proxy { get; }
	private IDisplay Display { get; }
	private ISubscriber<MonitorSubReddit> Subscriber { get; }

	private ConcurrentDictionary<string, SubReddit> SubReddits { get; } = new();

	/// <inheritdoc />
	public SubReddit this[string name] =>
		SubReddits.TryGetValue(name, out var subReddit)
			? subReddit
			: null;

	/// <inheritdoc />
	public IEnumerator<SubReddit> GetEnumerator() => SubReddits.Values.ToList().GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc />
	public async Task StartListenerAsync()
	{
		var subReddits = await Proxy.GetSubRedditsAsync();

		foreach (var subReddit in subReddits)
		{
			var name = subReddit?.Name;

			if (string.IsNullOrEmpty(name))
				continue;

			Display.Received<MonitorSubReddit>(name);

			SubReddits.TryAdd(name, new SubReddit { Name = name });
		}

		foreach (var monitor in Subscriber.Subscribe())
		{
			var name = monitor?.Name;

			if (string.IsNullOrEmpty(name))
				continue;

			Display.Received<MonitorSubReddit>(name);

			if (monitor.Start &&
				SubReddits.ContainsKey(name) == false)
			{
				SubReddits.TryAdd(name, new SubReddit { Name = name });
			}

			if (monitor.Stop &&
				SubReddits.ContainsKey(name))
			{
				SubReddits.TryRemove(name, out var _);
			}
		}
	}

	/// <inheritdoc />
	public async Task UpdateAsync(IEnumerable<SubReddit> subReddits)
	{
		foreach (var subReddit in subReddits)
		{
			var existing = this[subReddit.Name];

			if (existing is null)
				continue;

			existing.Posts =
				subReddit.Posts
					.Union(existing.Posts.Where(e => subReddit.Posts.Any(n => n.Id == e.Id)))
					.ToArray();

			await Proxy.UpdateAsync(existing);
		}
	}
}