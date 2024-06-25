using JackHenry.Proxy.CRUD.Interfaces;
using JackHenry.Proxy.CRUD.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JackHenry.Proxy.CRUD;

/// <inheritdoc />
internal class UrlResolver : IUrlResolver
{
	public UrlResolver(IOptions<List<CrudProxyOptions>> options)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(options.Value);

		Options = options.Value;
		BaseUrls = Array.Empty<string>();
	}

	private const string SwaggerJson = "swagger/v1/swagger.json";

	private List<CrudProxyOptions> Options { get; }

	/// <inheritdoc />
	public string[] BaseUrls { get; private set; }

	/// <inheritdoc />
	public async Task RefreshIfNeededAsync()
	{
		if (BaseUrls.Length > 0)
			return;

		await RefreshAsync();
	}

	/// <inheritdoc />
	public async Task RefreshAsync()
	{
		BaseUrls = Options.Select(o => o.BaseUrl).ToArray();

		var badUrls = new List<string>();

		foreach (var baseUrl in BaseUrls)
		{
			using var client =
				new HttpClient
				{
					BaseAddress = new Uri(baseUrl),
					Timeout = TimeSpan.FromMilliseconds(100),
				};

			try
			{
				await client.GetAsync(SwaggerJson);
			}
			catch
			{
				badUrls.Add(baseUrl);
			}
		}

		BaseUrls = BaseUrls.Where(url => badUrls.Contains(url) == false).ToArray();
	}
}