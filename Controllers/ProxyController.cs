using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Hosting.Internal;
using System;

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
        [ResponseCache(Duration = 2592000, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Get([FromQuery] string url)
        {
            string dir = configuration["dir"];
            string fileName = url.SanitizeName();
            var path = Path.Combine(dir, fileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    return PhysicalFile(path, "application/json");
                }
                else
                {
                    var response = await this.client.GetAsync(url);
                    using (var fs = new FileStream(path,
                        FileMode.CreateNew))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                    return PhysicalFile(path, "application/json");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
