using JackHenry.Proxy.CRUD.Interfaces;
using JackHenry.Proxy.CRUD.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace JackHenry.Proxy.CRUD.IoC;

public static class IoCExtensions
{
	public static IServiceCollection AddCrudProxy(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);

		return
			services
				.Configure<List<CrudProxyOptions>>(configuration.GetSection(CrudProxyOptions.Section))
				.AddSingleton<IUrlResolver, UrlResolver>()
				.AddScoped<ISignalRClient, SignalRClient>()
				.AddScoped<ICrudProxy, CrudProxy>();
	}
}