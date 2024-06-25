using JackHenry.Console.Dispatcher.Interfaces;
using JackHenry.Console.Extensions;
using JackHenry.Console.Interfaces;
using JackHenry.MessageBroker.Commands;
using JackHenry.MessageBroker.Interfaces;
using JackHenry.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace JackHenry.Console.Dispatcher;

/// <inheritdoc />
internal class CommandDispatcher : ICommandDispatcher
{
	public CommandDispatcher(
		IDisplay display,
		IPublisher<QuerySubReddit> publisher,
		ISubscriber<UpdateQueryRate> updateQueryRate,
		ISubscriber<MonitorSubReddit> monitorSubReddit)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(publisher);
		ArgumentNullException.ThrowIfNull(updateQueryRate);
		ArgumentNullException.ThrowIfNull(monitorSubReddit);

		Display = display;
		Publisher = publisher;

		Subscriber = 
			new Subscribers
			{
				UpdateQueryRate = updateQueryRate,
				MonitorSubReddit = monitorSubReddit,
			};
	}

	private struct Subscribers
	{
		public ISubscriber<UpdateQueryRate> UpdateQueryRate { get; set; }
		public ISubscriber<MonitorSubReddit> MonitorSubReddit { get; set; }
	}

	private IDisplay Display { get; }
	private IPublisher<QuerySubReddit> Publisher { get; }
	private Subscribers Subscriber { get; }
	private ConcurrentDictionary<string, SubReddit> SubReddits { get; }

	/// <inheritdoc />
	public async Task DispatchAsync()
	{
		var tasks =
			new[]
			{
				ListenForMonitorChanges(),
				ListenForRateAdjustments(),
			};

		await Task.WhenAll(tasks);
	}

	private Task ListenForMonitorChanges()
	{
		foreach (var monitor in Subscriber.MonitorSubReddit.Subscribe())
		{
			var name = monitor?.Name;

			if (string.IsNullOrEmpty(name))
				continue;

			Display.Received<MonitorSubReddit>(name);
		}

		return Task.CompletedTask;
	}

	private Task ListenForRateAdjustments()
	{
		foreach (var monitor in Subscriber.UpdateQueryRate.Subscribe())
		{
			var name = monitor?.Name;

			if (string.IsNullOrEmpty(name))
				continue;

			Display.Received<UpdateQueryRate>(name);
		}

		return Task.CompletedTask;
	}
}