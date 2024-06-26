using JackHenry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.Console.Reddit.Interfaces;

public interface ISubRedditCollection : IEnumerable<SubReddit>
{
	SubReddit this[string name] { get; }

	Task StartListenerAsync();
	Task UpdateAsync(IEnumerable<SubReddit> subReddits);
}