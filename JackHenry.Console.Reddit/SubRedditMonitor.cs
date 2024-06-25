using JackHenry.Console.Extensions;
using JackHenry.Console.Interfaces;
using JackHenry.Console.Reddit.Extensions;
using JackHenry.Console.Reddit.Interfaces;
using JackHenry.MessageBroker.Commands;
using JackHenry.MessageBroker.Interfaces;
using System;
using System.Threading.Tasks;

namespace JackHenry.Console.Reddit;

/// <inheritdoc />
internal class SubRedditMonitor : ISubRedditMonitor
{
	public SubRedditMonitor(
		IDisplay display,
		ISubscriber<QuerySubReddit> subscriber,
		IPublisher<UpdateSubReddit> publisher)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(subscriber);
		ArgumentNullException.ThrowIfNull(publisher);

		Display = display;
		Subscriber = subscriber;
		Publisher = publisher;
	}

	private IDisplay Display { get; }
	private ISubscriber<QuerySubReddit> Subscriber { get; }
	private IPublisher<UpdateSubReddit> Publisher { get; }

	/// <inheritdoc />
	public async Task MonitorAsync()
	{
		Display.Start();

		foreach (var query in Subscriber.Subscribe())
		{
			var subReddit = query?.SubReddit;

			if (subReddit is null)
				continue;

			Display.Received<QuerySubReddit>(subReddit.Name);

			Publisher.Publish(new UpdateSubReddit { SubReddit = subReddit });

			Display.Published<UpdateSubReddit>(subReddit.Name);
		}
	}
}