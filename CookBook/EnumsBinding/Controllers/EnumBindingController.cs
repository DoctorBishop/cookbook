using EnumsBinding.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EnumsBinding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnumBindingController : ControllerBase
    {

        private readonly ILogger<EnumBindingController> _logger;

        public EnumBindingController(ILogger<EnumBindingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Months> Get()
        {
            return Enum.GetValues<Months>();
        }

        [HttpPost("{month}")]
        public IActionResult Post(Months month)
        {
            return Ok();
        }
    }
}