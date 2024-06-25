using System.Collections.Generic;

namespace JackHenry.MessageBroker.Interfaces;

internal interface IFileSystem
{
	void Write<TCommand>(TCommand command) where TCommand : ICommand, new();

	IEnumerable<TCommand> ReadAll<TCommand>() where TCommand : ICommand, new();

	TCommand ReadNext<TCommand>() where TCommand : ICommand, new();
}