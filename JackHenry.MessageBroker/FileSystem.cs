using JackHenry.MessageBroker.Interfaces;
using JackHenry.MessageBroker.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JackHenry.MessageBroker;

internal class FileSystem : IFileSystem
{
	public FileSystem(IOptions<MessageBrokerOptions> options)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(options.Value);

		Options = options.Value;

		if (Directory.Exists(Options.Path) == false)
			Directory.CreateDirectory(Options.Path);
	}

	private MessageBrokerOptions Options { get; }

	private ConcurrentDictionary<Type, FileSystemWatcher> FileWatchers { get; } =
		new ConcurrentDictionary<Type, FileSystemWatcher>();

	public void Write<TCommand>(TCommand command)
		where TCommand : ICommand, new()
	{
		var fileName = GetFileName(command);

		File.WriteAllText(fileName, JsonConvert.SerializeObject(command));
	}

	public IEnumerable<TCommand> ReadAll<TCommand>()
		where TCommand : ICommand, new()
	{
		TCommand command;

		var path = GetDirectory<TCommand>();

		foreach (var fileName in Directory.GetFiles(path))
		{
			try
			{
				command = JsonConvert.DeserializeObject<TCommand>(File.ReadAllText(fileName));

				File.Delete(fileName);
			}
			catch
			{
				continue;
			}

			yield return command;
		}
	}

	public async Task<TCommand> ReadNextAsync<TCommand>()
		where TCommand : ICommand, new()
	{
		throw new NotImplementedException();
	}

	#region Helper methods

	private string GetFileName<TCommand>(TCommand command)
		where TCommand : ICommand, new()
	{
		var path = GetDirectory<TCommand>();

		return Path.Combine(path, command.Id);
	}

	private string GetDirectory<TCommand>()
		where TCommand : ICommand, new()
	{
		var path = Path.Combine(Options.Path, typeof(TCommand).Name);

		if (Directory.Exists(path) == false)
			Directory.CreateDirectory(path);

		return path;
	}

	#endregion
}