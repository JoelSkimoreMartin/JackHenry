using JackHenry.Models.Interfaces;

namespace JackHenry.MessageBroker.Interfaces;

public interface ISubscriber<TEvent>
	where TEvent : IEvent, new()
{

}