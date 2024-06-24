using JackHenry.MessageBroker.Interfaces;
using JackHenry.MessageBroker.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace JackHenry.MessageBroker.IoC;

public static class IoCExtensions
{
	public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);

		return
			services
				.Configure<List<MessageBrokerOptions>>(configuration.GetSection(MessageBrokerOptions.Section))
				.AddSingleton<IFileSystem, FileSystem>()
				.AddScoped(typeof(IPublisher<>), typeof(Publisher<>))
				.AddScoped(typeof(ISubscriber<>), typeof(Subscriber<>));
	}
}