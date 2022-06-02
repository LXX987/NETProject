using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.DLLs;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public string GetSomething(string id)
        {
            return "xxx";
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "Get value";
        }

        [HttpGet("{userod}")]
        public string GetUser(string userid)
        {
            return "GetUser value";
        }

        [HttpGet]
        public int ADDtest()
        {
            return ADDdll.myAdd(3,5);
        }
    }
}
