using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.MessageBroker.Interfaces;

internal interface IFileSystem
{
	void Write<TCommand>(TCommand command) where TCommand : ICommand, new();

	IEnumerable<TCommand> ReadAll<TCommand>() where TCommand : ICommand, new();

	Task<TCommand> ReadNextAsync<TCommand>() where TCommand : ICommand, new();
}