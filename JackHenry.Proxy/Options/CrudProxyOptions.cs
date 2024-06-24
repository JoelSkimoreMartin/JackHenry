namespace JackHenry.Proxy.Options;

internal class CrudProxyOptions
{
	public static string Section => "CrudProxies";

	public string Name { get; set; }

	public string BaseUrl { get; set; }
}