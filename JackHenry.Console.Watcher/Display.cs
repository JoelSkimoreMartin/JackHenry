using JackHenry.Console.Watcher.Interfaces;
using Output = System.Console;

namespace JackHenry.Console.Watcher;

internal class Display : IDisplay
{
	private const int Offset = 2;
	private int Tabs { get; set; }

	public IDisplay Clear()
	{
		Tabs = 0;

		Output.Clear();

		for (int i = 0; i < Offset; i++)
		{
			Output.WriteLine();
		}

		return this;
	}

	public IDisplay Indent()
	{
		Tabs++;

		return this;
	}

	public IDisplay Outdent()
	{
		Tabs--;

		if (Tabs < 0)
			Tabs = 0;

		return this;
	}

	public IDisplay WriteLine(string value = null)
	{
		value = new string('\t', Offset + Tabs) + (value ?? string.Empty);

		Output.WriteLine(value);

		return this;
	}
}