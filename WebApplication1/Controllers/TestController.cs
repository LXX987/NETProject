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
        public bool ADDtest(string email)
        {
            /*return ExamineCli.ExamineMailCli(email);*/
            /*QQMailSender.SendMailAsync(message, title, content);*/
            ClassLibrary1.QQMailSender.SendMailAsync("luo1391938@163.com", "团多多注册提醒", "您成功注册新账号，请记住账号和密码哟！");
            Console.WriteLine("Exiting");
            return true;
        }
        [HttpGet]
        public int ADDtest1()
        {
            COMdll cOMdll = new COMdll();
            cOMdll.comdll();
            return ADDdll.myAdd(3, 5);
        }
    }
}
