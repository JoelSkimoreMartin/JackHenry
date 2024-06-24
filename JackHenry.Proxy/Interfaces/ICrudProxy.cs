using JackHenry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.Proxy.Interfaces;

public interface IWebApiProxy
{
	Task<IEnumerable<SubReddit>> GetSubRedditsAsync();
	Task<SubReddit> GetSubRedditAsync(string name);
}