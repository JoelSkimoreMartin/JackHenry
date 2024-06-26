using JackHenry.Models;
using JackHenry.Proxy.Reddit.Interfaces;
using JackHenry.Proxy.Reddit.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reddit.NET.Client.Builder;
using Reddit.NET.Client.Models.Public.Listings.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JackHenry.Proxy.Reddit;

/// <inheritdoc />
internal class RedditProxy : IRedditProxy
{
	public RedditProxy(
		IOptions<RedditProxyOptions> options,
		ILoggerFactory loggerFactory,
		IHttpClientFactory httpClientFactory)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(options.Value);
		ArgumentNullException.ThrowIfNull(loggerFactory);
		ArgumentNullException.ThrowIfNull(httpClientFactory);

		Options = options.Value;
		LoggerFactory = loggerFactory;
		HttpClientFactory = httpClientFactory;
	}

	private RedditProxyOptions Options { get; }
	private ILoggerFactory LoggerFactory { get; }
	private IHttpClientFactory HttpClientFactory { get; }

	public async Task<IEnumerable<SubReddit>> QuerySubRedditAsync(string[] names)
	{
		try
		{
			var reddit =
				await RedditClientBuilder
					.New
					.WithHttpClientFactory(HttpClientFactory)
					.WithLoggerFactory(LoggerFactory)
					.WithCredentialsConfiguration(
						credentialsBuilder =>
						credentialsBuilder.ReadOnly(
							Options.ClientId,
							Options.ClientSecret,
							deviceId: Guid.NewGuid()))
					.BuildAsync();

			var subReddits = reddit.Subreddits(names);

			var submissions =
				subReddits.GetSubmissionsAsync(
					builder =>
					builder
						.WithSort(SubredditSubmissionSort.New)
						.WithMaximumItems(50 * names.Length));

			var results = 
				names
					.ToDictionary(
						name => name,
						name => new List<Post>());

			await foreach (var submission in submissions)
			{
				if (results.TryGetValue(submission?.Subreddit, out var posts) == false)
					continue;

				var post = posts.FirstOrDefault(p => p.Id == submission.Permalink);

				if (post is null)
				{
					post =
						new Post
						{
							Id = submission.Permalink,
							Title = submission.Title,
							Author = submission.Author,
						};

					posts.Add(post);
				}

				post.UpVotes = submission.Upvotes;
			}

			return
				results.Keys
					.Select(name =>
						new SubReddit
						{
							Name = name,
							Posts = results[name].ToArray(),
						});
		}
		catch (Exception x)
		{
			return Array.Empty<SubReddit>();
		}
	}
}