using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace JackHenry.Settings.IoC;

public static class IoCExtensions
{
	private const string FileName = "appsettings.json";

	public static IConfiguration BuildConfig(this IConfigurationBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		return
			builder
				.AddEmbedded()
				.AddFile()
				.Build();
	}

	private static IConfigurationBuilder AddFile(this IConfigurationBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		var basePath = Directory.GetCurrentDirectory();

		if (File.Exists(Path.Combine(basePath, FileName)) == false)
			return builder;

		return
			builder
				.SetBasePath(basePath)
				.AddJsonFile(FileName, optional: false, reloadOnChange: true);
	}

	private static IConfigurationBuilder AddEmbedded(this IConfigurationBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		var assembly = typeof(IoCExtensions).Assembly;
		var assemblyName = assembly.GetName().Name;

		var fullFileName = $"{assemblyName}.{FileName}";

		var input = assembly.GetManifestResourceStream(fullFileName);

		if (input is null)
			return builder;

		return builder.AddJsonStream(input);
	}
}