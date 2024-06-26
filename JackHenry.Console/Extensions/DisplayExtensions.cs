using JackHenry.Console.Interfaces;
using System;

namespace JackHenry.Console.Extensions;

public static class DisplayExtensions
{
	public static IDisplay Received<TCommand>(this IDisplay display, string subRedditName)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(subRedditName);

		return display.WriteLine($"Received {typeof(TCommand).Name} command for subreddit r/{subRedditName}.");
	}
	public static IDisplay Published<TCommand>(this IDisplay display, string subRedditName)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(subRedditName);

		return display.WriteLine($"Published {typeof(TCommand).Name} command for subreddit r/{subRedditName}.");
	}
}