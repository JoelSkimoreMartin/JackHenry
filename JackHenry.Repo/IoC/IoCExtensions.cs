using JackHenry.Repo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JackHenry.Repo.IoC;

public static class IoCExtensions
{
	public static IServiceCollection AddRepository(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services);

		return
			services
				.AddMemoryCache()
				.AddScoped<ISubRedditRepository, MemoryRepository>();
	}
}