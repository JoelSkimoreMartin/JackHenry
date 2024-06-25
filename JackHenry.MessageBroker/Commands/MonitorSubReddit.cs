namespace JackHenry.MessageBroker.Commands;

public class MonitorSubReddit : Command
{
	public string Name { get; set; }
	public bool Start { get; set; }
	public bool Stop { get; set; }
}