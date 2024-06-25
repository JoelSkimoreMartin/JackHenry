using System.Collections.Generic;

namespace JackHenry.MessageBroker.Interfaces;

public interface ISubscriber<TCommand>
	where TCommand : ICommand, new()
{
	IEnumerable<TCommand> Subscribe();
}