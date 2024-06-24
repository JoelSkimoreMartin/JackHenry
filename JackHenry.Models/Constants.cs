namespace JackHenry.Models;

/// <summary>
/// Constant values
/// </summary>
public static class Constants
{
	/// <summary>
	/// Constant values for SignalR
	/// </summary>
	public static class SignalR
	{
		/// <summary>
		/// Name of the SignalR hub
		/// </summary>
		public const string HubName = "CrudHub";

		/// <summary>
		/// Name of the SignalR notifier method
		/// </summary>
		public const string Notifier = "NotifyUpdate";

		/// <summary>
		/// Name of the SignalR reciever method
		/// </summary>
		public const string Receiver = "ReceiveUpdateNotification";
	}
}