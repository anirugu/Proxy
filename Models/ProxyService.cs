using Microsoft.AspNetCore.Mvc;

namespace Proxy.Models;

public class ProxyService : IProxyService
{
    private IHttpClientFactory _clientFactory;

    public ProxyService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public async Task<string> GetResult(string url)
    {
        string fileName = Utils.GetFilePath(url);
        try
        {
            if (!System.IO.File.Exists(fileName))
            {
                var client = this._clientFactory.CreateClient();
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (var fs = new FileStream(fileName,
                               FileMode.CreateNew))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
                else
                {
                    throw new Exception("Server failed to respond");
                }
            }
            return await Task.FromResult(fileName);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}