using JackHenry.Console.Interfaces;
using JackHenry.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JackHenry.Console.Watcher.ExtensionMethods;

internal static class DisplayExtensions
{
	public static IDisplay Starting(this IDisplay display)
	{
		ArgumentNullException.ThrowIfNull(display);

		display.Clear();
		display.WriteLine("Starting SignalR Client...");

		return display;
	}

	public static IDisplay Update(this IDisplay display, IEnumerable<SubReddit> subReddits)
	{
		ArgumentNullException.ThrowIfNull(display);

		display.Clear();

		if (subReddits?.Any() != true)
		{
			display.WriteLine("No subreddits");
			return display;
		}

		display.WriteLine("Subreddits:");

		foreach (var subReddit in subReddits)
		{
			display
				.Indent()
				.WriteLine($"- r/{subReddit.Name}:")
				.Indent()
				.WriteLine("Posts:")
				.Indent();

			foreach (var post in subReddit.MostUpVotedPosts)
			{
				display.WriteLine($"- {post.UpVotes} up votes for '{post.Title}'");
			}

			display
				.Outdent()
				.Outdent()
				.Outdent();
		}

		return display;
	}
}