using JackHenry.Models.Interfaces;

namespace JackHenry.MessageBroker.Interfaces;

public interface IPublisher<TEvent>
	where TEvent : IEvent, new()
{
}