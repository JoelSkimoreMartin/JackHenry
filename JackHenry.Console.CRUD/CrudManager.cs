using JackHenry.Console.CRUD.Interfaces;
using JackHenry.Console.Extensions;
using JackHenry.Console.Interfaces;
using JackHenry.MessageBroker.Commands;
using JackHenry.MessageBroker.Interfaces;
using JackHenry.Proxy.CRUD.Interfaces;
using System;
using System.Threading.Tasks;

namespace JackHenry.Console.CRUD;

/// <inheritdoc />
internal class CrudManager : ICrudManager
{
	public CrudManager(
		IDisplay display,
		ICrudProxy proxy,
		ISubscriber<UpdateSubReddit> subscriber)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(proxy);
		ArgumentNullException.ThrowIfNull(subscriber);

		Display = display;
		Proxy = proxy;
		Subscriber = subscriber;
	}

	private IDisplay Display { get; }
	private ICrudProxy Proxy { get; }
	private ISubscriber<UpdateSubReddit> Subscriber { get; }

	/// <inheritdoc />
	public async Task ManageAsync()
	{
		Display.Clear();
		Display.WriteLine("Started");
		Display.WriteLine("Waiting for commands");

		foreach (var update in Subscriber.Subscribe())
		{
			if (update?.SubReddit is null)
				continue;

			Display.Received<UpdateSubReddit>(update.SubReddit?.Name);

			await Proxy.UpdateAsync(update?.SubReddit);
		}
	}
}