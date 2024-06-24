using System;

namespace JackHenry.MessageBroker.Interfaces;

public interface ICommand
{
	string Id { get; set; }
	DateTime Created { get; set; }
}