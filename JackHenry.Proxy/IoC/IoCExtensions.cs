using JackHenry.Proxy.Interfaces;
using JackHenry.Proxy.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace JackHenry.Proxy.IoC;

public static class IoCExtensions
{
	public static IServiceCollection AddWebApiProxy(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);

		return
			services
				.Configure<List<CrudProxyOptions>>(configuration.GetSection(CrudProxyOptions.Section))
				.AddSingleton<UrlResolver>()
				.AddScoped<ISignalRClient, SignalRClient>()
				.AddScoped<IWebApiProxy, CrudProxy>();
	}
}