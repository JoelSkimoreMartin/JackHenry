using JackHenry.Models;
using JackHenry.Repo.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackHenry.Repo;

internal class MemoryRepository : ISubRedditRepository
{
	public MemoryRepository(IMemoryCache memoryCache)
	{
		ArgumentNullException.ThrowIfNull(memoryCache);

		Cache = memoryCache;
	}

	private IMemoryCache Cache { get; }

	private ConcurrentDictionary<string, SubReddit> SubReddits { get; set; }

	private async Task<ConcurrentDictionary<string, SubReddit>> GetSubRedditsAsync()
	{
		if (SubReddits is not null)
			return SubReddits;

		SubReddits = await Cache.GetOrCreateAsync(nameof(SubReddits), entry => Task.FromResult(new ConcurrentDictionary<string, SubReddit>()));

		return SubReddits;
	}

	public async Task<IEnumerable<SubReddit>> GetAllAsync()
	{
		var subReddits = await GetSubRedditsAsync();

		return subReddits.Values.ToArray();
	}

	public async Task<SubReddit> GetAsync(string name)
	{
		var subReddits = await GetSubRedditsAsync();

		subReddits.TryGetValue(name.ToUpper(), out var subreddit);

		return subreddit;
	}

	public async Task UpsertAsync(SubReddit subReddit)
	{
		var subReddits = await GetSubRedditsAsync();

		subReddits.TryGetValue(subReddit.Name, out var existing);

		if (existing is null)
		{
			subReddits.TryAdd(subReddit.Name.ToUpper(), subReddit);
		}
        else
        {
			subReddits.TryUpdate(subReddit.Name.ToUpper(), subReddit, existing);
		}
	}

	public async Task DeleteAsync(string name)
	{
		var subReddits = await GetSubRedditsAsync();

		subReddits.TryRemove(name.ToUpper(), out var _);
	}
}