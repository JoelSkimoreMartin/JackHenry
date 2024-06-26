namespace JackHenry.Proxy.Reddit.Options
{
	internal class RedditProxyOptions
	{
		public static string Section => "RedditProxy";

		public string UserId { get; set; }

		public string ClientId { get; set; }

		public string ClientSecret { get; set; }
	}
}