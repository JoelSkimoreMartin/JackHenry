namespace JackHenry.MessageBroker.Interfaces;

public interface IPublisher<TCommand>
	where TCommand : ICommand, new()
{
}