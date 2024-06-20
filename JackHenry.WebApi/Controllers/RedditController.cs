using JackHenry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JackHenry.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RedditController : ControllerBase
	{
		private ILogger<RedditController> Logger { get; }

		public RedditController(ILogger<RedditController> logger)
		{
			Logger = logger;
		}

		[HttpGet("subreddits")]
		public async Task<IEnumerable<SubReddit>> GetSubredditsAsync()
		{
			return Array.Empty<SubReddit>();
		}

		[HttpGet("subreddits/{name}")]
		public async Task<IEnumerable<SubReddit>> GetSubredditAsync(string name)
		{
			return null;
		}
	}
}