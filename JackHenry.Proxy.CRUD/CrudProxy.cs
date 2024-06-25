using JackHenry.Models;
using JackHenry.Proxy.CRUD.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace JackHenry.Proxy.CRUD;

/// <inheritdoc />
internal class CrudProxy : ICrudProxy
{
	public CrudProxy(IUrlResolver urlResolver)
	{
		ArgumentNullException.ThrowIfNull(urlResolver);

		UrlResolver = urlResolver;
	}

	private IUrlResolver UrlResolver { get; set; }

	private const string SubRedditsEndpoint = "api/subreddits";

	/// <inheritdoc />
	public async Task<IEnumerable<SubReddit>> GetSubRedditsAsync() =>
		await GetAsync<IEnumerable<SubReddit>>(SubRedditsEndpoint)
		??
		Array.Empty<SubReddit>();

	/// <inheritdoc />
	public async Task<SubReddit> GetSubRedditAsync(string name) => await GetAsync<SubReddit>($"{SubRedditsEndpoint}/{name}");

	/// <inheritdoc />
	public async Task UpdateAsync(SubReddit subReddit)
	{
		if (subReddit is null)
			return;

		var json = JsonConvert.SerializeObject(subReddit);

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
				await client.PostAsJsonAsync(HttpUtility.UrlEncode(subReddit.Name), json);

				return;
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("connection"))
					await UrlResolver.RefreshAsync();
			}
		}
	}

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