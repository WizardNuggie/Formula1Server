using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Formula1Server.Models;

namespace Formula1Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class F1APIController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private F1DBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public F1APIController(F1DBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }
    }
}
