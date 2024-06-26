using JackHenry.Proxy.Reddit.Interfaces;
using JackHenry.Proxy.Reddit.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reddit.NET.Client.Builder;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JackHenry.Proxy.Reddit.IoC;

public static class IoCExtensions
{
	public static IServiceCollection AddRedditProxy(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);

		var assembly = Assembly.GetEntryAssembly().GetName();

		var userId = configuration[$"{RedditProxyOptions.Section}:{nameof(RedditProxyOptions.UserId)}"];

		return
			services
				.AddScoped<IRedditProxy, RedditProxy>()
				.Configure<RedditProxyOptions>(configuration.GetSection(RedditProxyOptions.Section))
				.AddRedditHttpClient(userAgent: $"{Environment.OSVersion.Platform}:{assembly.Name}:v{assembly.Version} (by {userId})");
	}
}