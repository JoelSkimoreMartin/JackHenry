using JackHenry.MessageBroker.Interfaces;
using JackHenry.MessageBroker.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace JackHenry.MessageBroker;

/// <inheritdoc />
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

	/// <inheritdoc />
	public void Write<TCommand>(TCommand command)
		where TCommand : ICommand, new()
	{
		var fileName = GetFileName(command);

		File.WriteAllText(fileName, JsonConvert.SerializeObject(command));
	}

	/// <inheritdoc />
	public IEnumerable<TCommand> ReadAll<TCommand>()
		where TCommand : ICommand, new()
	{
		var path = GetDirectory<TCommand>();

		foreach (var fileName in Directory.GetFiles(path))
		{
			var command = Read<TCommand>(fileName);

			if (command is null)
				continue;

			yield return command;
		}
	}

	/// <inheritdoc />
	public TCommand ReadNext<TCommand>()
		where TCommand : ICommand, new()
	{
		var fileName = string.Empty;
		var signal = new EventWaitHandle(false, EventResetMode.AutoReset);

		var path = GetDirectory<TCommand>();

		using var watcher =
			new FileSystemWatcher
			{
				Path = path,
				NotifyFilter = NotifyFilters.LastWrite,
				Filter = "*",
				EnableRaisingEvents = true,
			};

		watcher.Changed +=
			new FileSystemEventHandler(
				(sender, args) =>
				{
					fileName = args.FullPath;
					signal.Set();
				});

		signal.WaitOne();

		return Read<TCommand>(fileName);
	}

	#region Helper methods

	private TCommand Read<TCommand>(string fileName)
		where TCommand : ICommand, new()
	{
		TCommand command;
		try
		{
			command = JsonConvert.DeserializeObject<TCommand>(File.ReadAllText(fileName));

			File.Delete(fileName);
		}
		catch
		{
			return default;
		}

		return command;
	}

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