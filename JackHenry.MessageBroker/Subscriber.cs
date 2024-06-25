using JackHenry.MessageBroker.Interfaces;
using System;
using System.Collections.Generic;

namespace JackHenry.MessageBroker;

/// <inheritdoc />
internal class Subscriber<TCommand> : ISubscriber<TCommand>
	where TCommand : ICommand, new()
{
	public Subscriber(IFileSystem fileSystem)
	{
		ArgumentNullException.ThrowIfNull(fileSystem);

		FileSystem = fileSystem;
	}

	private IFileSystem FileSystem { get; }

	/// <inheritdoc />
	public IEnumerable<TCommand> Subscribe()
	{
		// Read the existing commands
		foreach (var command in FileSystem.ReadAll<TCommand>())
		{
			if (command is null)
				continue;

			yield return command;
		}

		// Listen for the incoming commands
		while (true)
		{
			var command = FileSystem.ReadNext<TCommand>();

			if (command is null)
				continue;

			yield return command;
		}
	}
}