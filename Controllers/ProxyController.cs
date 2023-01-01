using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;
using Proxy.Models;

namespace Proxy.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        //private readonly IHttpClientFactory clientFactory;
        //private readonly IConfiguration configuration;
        private readonly IProxyService _proxyService;

        public ProxyController(IProxyService proxyService)
        {
            _proxyService = proxyService;
        }

        [HttpGet]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Get([FromQuery] string url)
        {
            var fileName = await _proxyService.GetResult(url);
            return PhysicalFile(fileName, "application/json");
        }

        [HttpGet("GetPath")]
        public async Task<IActionResult> GetTempStorage([FromQuery] string variable)
        {
            return Ok(System.Environment.ExpandEnvironmentVariables(variable));
        }

    }
}
