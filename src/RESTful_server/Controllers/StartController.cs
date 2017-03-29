using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTful_server.Controllers
{
    [EnableCors("TestPolicy")]
    [Route("api/[controller]")]
    public class StartController : Controller
    {
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return "Server is available at the following address: ' " + HttpContext.Request.Host.Value + " ' ";
        }
    }
}
