using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Proxy.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public ProxyController(HttpClient client, IConfiguration config)
        {
            this.client = client;
            this.configuration = config;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] string url)
        {
            string dir = configuration["dir"];
            string fileName = url.SanitizeName();
            var path = Path.Combine(dir, fileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    return await System.IO.File.ReadAllTextAsync(path);
                }
                else
                {
                    string response = await this.client.GetStringAsync(url);
                    System.IO.File.WriteAllText(path, response);
                    return response;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    }
}
