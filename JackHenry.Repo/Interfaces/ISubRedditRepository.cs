using JackHenry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.Repo.Interfaces;

public interface ISubRedditRepository
{
	Task<IEnumerable<SubReddit>> GetAllAsync();

	Task<SubReddit> GetAsync(string name);

	Task UpsertAsync(SubReddit subReddit);

	Task DeleteAsync(string name);
}