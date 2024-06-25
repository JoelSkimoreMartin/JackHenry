using JackHenry.Models;

namespace JackHenry.MessageBroker.Commands;

public class QuerySubReddit : Command
{
	public SubReddit SubReddit { get; set; }
}