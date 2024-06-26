using JackHenry.Console.Interfaces;
using JackHenry.MessageBroker.Commands;
using System;

namespace JackHenry.Console.Reddit.Extensions;

internal static class DisplayExtensions
{
	public static IDisplay Start(this IDisplay display)
	{
		ArgumentNullException.ThrowIfNull(display);

		return
			display
				.Clear()
				.WriteLine($"Start listening for {nameof(MonitorSubReddit)} commands.");
	}
}