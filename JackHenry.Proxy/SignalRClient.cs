using JackHenry.Models;
using JackHenry.Proxy.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Proxy;

internal class SignalRClient : ISignalRClient
{
	public SignalRClient(UrlResolver urlResolver)
	{
		ArgumentNullException.ThrowIfNull(urlResolver);

		UrlResolver = urlResolver;
	}

	private UrlResolver UrlResolver { get; set; }

	public EventWaitHandle Signal { get; } = new EventWaitHandle(false, EventResetMode.AutoReset);

	public async Task StartAsync()
	{
		await UrlResolver.RefreshIfNeededAsync();

		var connections =
			UrlResolver
				.BaseUrls
				.Select(baseUrl => $"{baseUrl}/{Constants.SignalR.HubName}")
				.Select(url => new HubConnectionBuilder().WithUrl(url).Build())
				.ToArray();

		foreach (var connection in connections)
		{
			connection.On(Constants.SignalR.Receiver, (string name) => Signal.Set());
			connection.KeepAliveInterval = TimeSpan.FromSeconds(1);

			try
			{
				await connection.StartAsync();
			}
			catch
			{
				await UrlResolver.RefreshAsync();

				Thread.Sleep(500);

				await StartAsync();
			}
		}
	}
}