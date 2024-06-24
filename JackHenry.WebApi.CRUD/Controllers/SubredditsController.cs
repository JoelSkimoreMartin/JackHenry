using JackHenry.Models;
using JackHenry.Repo.Interfaces;
using JackHenry.WebApi.CRUD.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace JackHenry.WebApi.CRUD.Controllers
{
	/// <summary>
	/// Reddit controller
	/// </summary>
	[ApiController]
	[Route("api/subreddits")]
	public class SubredditsController : ControllerBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="repository">repository for storing results</param>
		/// <param name="signalR">SignalR context</param>
		public SubredditsController(ISubRedditRepository repository, IHubContext<CrudHub> signalR)
		{
			ArgumentNullException.ThrowIfNull(repository);
			ArgumentNullException.ThrowIfNull(signalR);

			Repo = repository;
			SignalR = signalR;
		}

		private ISubRedditRepository Repo { get; }
		private IHubContext<CrudHub> SignalR { get; }

		private string BaseUrl => $"{Request.Scheme}://{Request.Host.Value}";

		/// <summary>
		/// Get all subreddits
		/// </summary>
		/// <remarks>
		/// Gets all the subreddits being monitored
		/// </remarks>
		/// <response code="200">Returns all the subreddits being monitored</response>
		/// <response code="404">No subreddits being monitored exist</response>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<SubReddit>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<SubReddit>>> GetAllAsync()
		{
			var subReddits = await Repo.GetAllAsync();

			if (subReddits?.Any() != true)
				return NotFound();

			return Ok(subReddits);
		}

		/// <summary>
		/// Get a subreddit
		/// </summary>
		/// <param name="name">Name of the subreddit being monitored</param>
		/// <remarks>
		/// Gets the subreddits being monitored with the supplied <paramref name="name"/>
		/// </remarks>
		/// <response code="200">Returns the subreddit being monitored with <paramref name="name"/></response>
		/// <response code="404">No subreddit being monitored with <paramref name="name"/> exists</response>
		[HttpGet("{name}")]
		[ProducesResponseType(typeof(SubReddit), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<SubReddit>> GetAsync(string name)
		{
			var subReddit = await Repo.GetAsync(name);

			if (subReddit is null)
				return NotFound();

			return Ok(subReddit);
		}

		/// <summary>
		/// Create a subreddit
		/// </summary>
		/// <param name="name">Name of the subreddit to create</param>
		/// <remarks>
		/// Create a subreddit to be monitored
		/// </remarks>
		/// <response code="201">Successfully created a subreddit to be monitored</response>
		/// <response code="409">The subreddit with the supplied <paramref name="name"/> already exists</response>
		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(SubReddit), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
		public async Task<ActionResult<SubReddit>> CreateAsync(string name)
		{
			var existing = await Repo.GetAsync(name);

			if (existing is not null)
				return Conflict();

			var subReddit =
				new SubReddit
				{
					Name = name,
				};

			await Repo.UpsertAsync(subReddit);

			await SignalAsync(name);

			var url = $"{BaseUrl}/api/subreddits/{subReddit.Name.ToLower()}";

			return Created(new Uri(url, UriKind.RelativeOrAbsolute), subReddit);
		}

		/// <summary>
		/// Update a subreddit
		/// </summary>
		/// <param name="name">Name of the subreddit</param>
		/// <param name="subReddit">Subreddit to update</param>
		/// <remarks>
		/// Update a subreddit to be monitored
		/// </remarks>
		/// <response code="200">Successfully updated a subreddit to be monitored</response>
		/// <response code="404">The supplied <paramref name="subReddit"/> does not exist</response>
		[HttpPut("{name}")]
		[ProducesResponseType(typeof(SubReddit), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> UpdateAsync(string name, SubReddit subReddit)
		{
			var existing = await Repo.GetAsync(name);

			if (existing is null)
				return NotFound();

			await Repo.UpsertAsync(subReddit);

			await SignalAsync(name);

			return Ok(subReddit.Name);
		}

		/// <summary>
		/// Remove a subreddit
		/// </summary>
		/// <remarks>
		/// Delete a subreddit to be monitored
		/// </remarks>
		/// <param name="name">Name of the subreddit</param>
		/// <response code="200">Successfully deleted a subreddit to be monitored</response>
		/// <response code="404">The subreddit for the supplied <paramref name="name"/> does not exist</response>
		[HttpDelete("{name}")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<string>> DeleteAsync(string name)
		{
			var existing = await Repo.GetAsync(name);

			if (existing is null)
				return NotFound();

			await Repo.DeleteAsync(name);

			await SignalAsync(name);

			return Ok(name);
		}

		private async Task SignalAsync(string name) => await SignalR.Clients.All.SendAsync(Constants.SignalR.Receiver, name);
	}
}