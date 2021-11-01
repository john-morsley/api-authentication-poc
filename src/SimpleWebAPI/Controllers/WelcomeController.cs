using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WelcomeController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("not-secret")]
        public string NotSecret()
        {
            return "Hello";
        }

        [HttpGet]
        [Route("secret")]
        public string Secret()
        {
            return "Hello";
        }
    }
}