namespace Proxy.Models;

public interface IProxyService
{
    Task<string> GetResult(string url);
}