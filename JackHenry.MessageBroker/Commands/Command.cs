using JackHenry.MessageBroker.Interfaces;
using System;

namespace JackHenry.MessageBroker.Commands;

public abstract class Command : ICommand
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public DateTime Created { get; set; } = DateTime.Now;
}