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
        private readonly IHttpClientFactory clientFactory;
        private readonly IConfiguration configuration;

        public ProxyController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            this.clientFactory = clientFactory;
            this.configuration = config;
        }

        [HttpGet]
        [ResponseCache(Duration = 2592000, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Get([FromQuery] string url)
        {
            try
            {
                var client = this.clientFactory.CreateClient();
                var response = await client.GetStreamAsync(url);
                return File(response, "application/json");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
