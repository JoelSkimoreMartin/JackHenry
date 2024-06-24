using System;
using System.Linq;

namespace JackHenry.Models;

/// <summary>
/// Statistics of a subreddit
/// </summary>
public class SubReddit
{
	/// <summary>
	/// Name of the subreddit
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Posts on the subreddit
	/// </summary>
	public Post[] Posts { get; set; } = Array.Empty<Post>();

	/// <summary>
	/// Most up voted posts to display
	/// </summary>
	public const int MostUpVotedDisplayCount = 10;

	/// <summary>
	/// Posts on the subreddit
	/// </summary>
	public Post[] MostUpVotedPosts =>
		Posts?
			.OrderBy(p => p.UpVotes)
			.Take(MostUpVotedDisplayCount)
			.ToArray()
		??
		Array.Empty<Post>();
}