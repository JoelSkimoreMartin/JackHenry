using JackHenry.Models;

namespace JackHenry.MessageBroker.Commands;

public class UpdateSubReddit : Command
{
	public SubReddit SubReddit { get; set; }
}