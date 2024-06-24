using JackHenry.Console.Watcher.Interfaces;
using JackHenry.Models;
using JackHenry.Proxy.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JackHenry.Console.Watcher;

public class Watcher : IWatcher
{
	public Watcher(IDisplay display, IWebApiProxy proxy, ISignalRClient signalR)
	{
		ArgumentNullException.ThrowIfNull(display);
		ArgumentNullException.ThrowIfNull(proxy);
		ArgumentNullException.ThrowIfNull(signalR);

		Proxy = proxy;
		SignalR = signalR;
		Display = display;
	}

	private IWebApiProxy Proxy { get; }
	private ISignalRClient SignalR { get; }
	private IDisplay Display { get; }

	public async Task StartAsync()
	{
		Display.Clear();
		Display.WriteLine("Starting SignalR Client...");

		await SignalR.StartAsync();
	}

	public async Task WatchAsync()
	{
		var subReddits = await Proxy.GetSubRedditsAsync();

		UpdateDisplay(subReddits.ToArray());

		SignalR.Signal.WaitOne();
	}

	private void UpdateDisplay(SubReddit[] subReddits)
	{
		Display.Clear();

		if (subReddits.Any() == false)
		{
			Display.WriteLine("No subreddits");
			return;
		}

		Display.WriteLine("Subreddits:");

		foreach (var subReddit in subReddits)
		{
			Display
				.Indent()
				.WriteLine($"- r/{subReddit.Name}:")
				.Indent()
				.WriteLine("Posts:")
				.Indent();

			foreach (var post in subReddit.MostUpVotedPosts)
			{
				Display.WriteLine($"- {post.UpVotes} up votes for '{post.Title}'");
			}

			Display
				.Outdent()
				.Outdent()
				.Outdent();
		}
	}
}