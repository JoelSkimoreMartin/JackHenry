namespace JackHenry.MessageBroker.Options;

internal class MessageBrokerOptions
{
	public static string Section => "MessageBroker";

	public string Path { get; set; } = "C:\\Dev\\Assessments\\JoelMartin\\Events";
}