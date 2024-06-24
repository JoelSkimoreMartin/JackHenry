using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Proxy.Interfaces;

public interface ISignalRClient
{
	EventWaitHandle Signal { get; }

	Task StartAsync();
}