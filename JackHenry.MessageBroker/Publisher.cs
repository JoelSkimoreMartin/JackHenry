using JackHenry.Models.Interfaces;

namespace JackHenry.MessageBroker;

internal class Publisher<TEvent>
	where TEvent : IEvent, new()
{
}