using JackHenry.Models;
using JackHenry.Proxy.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace JackHenry.Proxy;

internal class CrudProxy : IWebApiProxy
{
	public CrudProxy(UrlResolver urlResolver)
	{
		ArgumentNullException.ThrowIfNull(urlResolver);

		UrlResolver = urlResolver;
	}

	private UrlResolver UrlResolver { get; set; }

	private const string SubRedditsEndpoint = "api/subreddits";

	public async Task<IEnumerable<SubReddit>> GetSubRedditsAsync() =>
		await GetAsync<IEnumerable<SubReddit>>(SubRedditsEndpoint)
		??
		Array.Empty<SubReddit>();

	public async Task<SubReddit> GetSubRedditAsync(string name) => await GetAsync<SubReddit>($"{SubRedditsEndpoint}/{name}");

	#region Helper methods

	private async Task<T> GetAsync<T>(string requestUri)
	{
		await UrlResolver.RefreshIfNeededAsync();

		var baseUrls = UrlResolver.BaseUrls;

		foreach (var baseUrl in baseUrls)
		{
			using var client =
				new HttpClient
				{
					BaseAddress = new Uri(baseUrl),
				};

			try
			{
				var response = await client.GetAsync(requestUri);

				return await response.Content.ReadFromJsonAsync<T>();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("connection"))
					await UrlResolver.RefreshAsync();
			}
		}

		return default;
	}

	#endregion
}