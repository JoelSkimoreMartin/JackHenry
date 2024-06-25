using JackHenry.Console.Interfaces;
using Output = System.Console;

namespace JackHenry.Console;

/// <inheritdoc />
internal class Display : IDisplay
{
	private const int Offset = 2;
	private int Tabs { get; set; }

	/// <inheritdoc />
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

	/// <inheritdoc />
	public IDisplay Indent()
	{
		Tabs++;

		return this;
	}

	/// <inheritdoc />
	public IDisplay Outdent()
	{
		Tabs--;

		if (Tabs < 0)
			Tabs = 0;

		return this;
	}

	/// <inheritdoc />
	public IDisplay WriteLine(string value = null)
	{
		value = new string('\t', Offset + Tabs) + (value ?? string.Empty);

		Output.WriteLine(value);

		return this;
	}
}