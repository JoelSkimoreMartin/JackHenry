using JackHenry.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JackHenry.WebApi.CRUD.Hubs;

/// <summary>
/// SignalR Hub for the SubReddits
/// </summary>
public class CrudHub : Hub
{
	/// <summary>
	/// Notify of an update to a subreddit's info
	/// </summary>
	/// <param name="name">Name of the subreddit</param>
	public async Task NotifyUpdate(string name)
	{
		await Clients.All.SendAsync(Constants.SignalR.Receiver, name);
	}
}