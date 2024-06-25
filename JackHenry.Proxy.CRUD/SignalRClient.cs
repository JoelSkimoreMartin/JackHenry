using JackHenry.Models;
using JackHenry.Proxy.CRUD.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Proxy.CRUD;

/// <inheritdoc />
internal class SignalRClient : ISignalRClient
{
	public SignalRClient(IUrlResolver urlResolver)
	{
		ArgumentNullException.ThrowIfNull(urlResolver);

		UrlResolver = urlResolver;
	}

	private IUrlResolver UrlResolver { get; set; }

	public EventWaitHandle Signal { get; } = new EventWaitHandle(false, EventResetMode.AutoReset);

	/// <inheritdoc />
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