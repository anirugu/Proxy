using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;

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
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Get([FromQuery] string url)
        {
            string fileName = Utils.GetFilePath(url);
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    return PhysicalFile(fileName, "application/json");
                }
                else
                {
                    var client = this.clientFactory.CreateClient();
                    var response = await client.GetAsync(url);
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (var fs = new FileStream(fileName,
                       FileMode.CreateNew))
                        {
                            await response.Content.CopyToAsync(fs);
                        }
                        return PhysicalFile(fileName, "application/json");
                    }
                    else
                    {
                        throw new Exception("Server failed to respond");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetPath")]
        public async Task<IActionResult> GetTempStorage([FromQuery] string variable)
        {
            return Ok(System.Environment.ExpandEnvironmentVariables(variable));
        }

    }
}
