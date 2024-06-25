using System.Threading.Tasks;

namespace JackHenry.Proxy.CRUD.Interfaces;

internal interface IUrlResolver
{
	string[] BaseUrls { get; }

	Task RefreshAsync();

	Task RefreshIfNeededAsync();
}