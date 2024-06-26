using JackHenry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JackHenry.Proxy.Reddit.Interfaces;

/// <summary>
/// Proxy for the Reddit API
/// </summary>
public interface IRedditProxy
{
	/// <summary>
	/// Query the latest posts for the supplied subreddits
	/// </summary>
	/// <param name="names">collection of subreddit names</param>
	/// <returns><see cref="SubReddit"/>s populated with query info</returns>
	Task<IEnumerable<SubReddit>> QuerySubRedditAsync(string[] names);
}