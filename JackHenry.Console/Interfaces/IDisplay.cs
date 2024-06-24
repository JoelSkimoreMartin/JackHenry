namespace JackHenry.Console.Interfaces;

public interface IDisplay
{
	IDisplay Clear();
	IDisplay Indent();
	IDisplay Outdent();
	IDisplay WriteLine(string value = null);
}