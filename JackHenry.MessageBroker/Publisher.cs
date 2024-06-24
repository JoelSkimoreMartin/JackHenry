using JackHenry.MessageBroker.Interfaces;

namespace JackHenry.MessageBroker;

internal class Publisher<TCommand>
	where TCommand : ICommand, new()
{
}