using System.Threading;
using System.Threading.Tasks;

namespace JackHenry.Proxy.CRUD.Interfaces;

public interface ISignalRClient
{
	EventWaitHandle Signal { get; }

	Task StartAsync();
}