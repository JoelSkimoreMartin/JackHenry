using JackHenry.MessageBroker.Interfaces;

namespace JackHenry.MessageBroker;

internal class Subscriber<TCommand>
	where TCommand : ICommand, new()
{
}