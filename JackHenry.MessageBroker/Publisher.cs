using JackHenry.MessageBroker.Interfaces;
using System;

namespace JackHenry.MessageBroker;

/// <inheritdoc />
internal class Publisher<TCommand> : IPublisher<TCommand>
	where TCommand : ICommand, new()
{
	public Publisher(IFileSystem fileSystem)
	{
		ArgumentNullException.ThrowIfNull(fileSystem);

		FileSystem = fileSystem;
	}

	private IFileSystem FileSystem { get; }

	/// <inheritdoc />
	public void Publish(TCommand command) => FileSystem.Write(command);
}