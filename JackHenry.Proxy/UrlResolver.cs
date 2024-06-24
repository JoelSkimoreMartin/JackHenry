using JackHenry.Proxy.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JackHenry.Proxy
{
	internal class UrlResolver
	{
		public UrlResolver(IOptions<List<CrudProxyOptions>> options)
		{
			ArgumentNullException.ThrowIfNull(options);
			ArgumentNullException.ThrowIfNull(options.Value);

			Options = options.Value;
			BaseUrls = Array.Empty<string>();
		}

		private const string SwaggerJson = "swagger/v1/swagger.json";

		public List<CrudProxyOptions> Options { get; private set; }

		public string[] BaseUrls { get; private set; }

		public async Task RefreshIfNeededAsync()
		{
			if (BaseUrls.Length > 0)
				return;

			await RefreshAsync();
		}

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
}