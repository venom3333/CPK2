using CPK.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CPK.Spa.Server.Controllers
{
    [Route("api/v1/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IOptionsSnapshot<ConfigModel> _configuration;

        public ConfigController(IOptionsSnapshot<ConfigModel> configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<controller>
        [HttpGet]
        public ConfigModel Get()
        {
            return _configuration.Value;
        }
    }

}
