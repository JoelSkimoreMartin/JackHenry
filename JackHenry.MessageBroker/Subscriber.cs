using JackHenry.Models.Interfaces;

namespace JackHenry.MessageBroker;

internal class Subscriber<TEvent>
	where TEvent : IEvent, new()
{
}