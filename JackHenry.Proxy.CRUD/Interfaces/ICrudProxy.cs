using JackHenry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.Proxy.CRUD.Interfaces;

public interface ICrudProxy
{
	Task<IEnumerable<SubReddit>> GetSubRedditsAsync();
	Task<SubReddit> GetSubRedditAsync(string name);
	Task UpdateAsync(SubReddit subReddit);
}