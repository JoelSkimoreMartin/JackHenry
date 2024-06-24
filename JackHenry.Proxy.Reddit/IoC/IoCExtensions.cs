using JackHenry.Proxy.Reddit.Interfaces;
using JackHenry.Proxy.Reddit.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace JackHenry.Proxy.Reddit.IoC;

public static class IoCExtensions
{
	public static IServiceCollection AddRedditProxy(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);

		return
			services
				.Configure<List<RedditProxyOptions>>(configuration.GetSection(RedditProxyOptions.Section))
				.AddScoped<IRedditProxy, RedditProxy>();
	}
}