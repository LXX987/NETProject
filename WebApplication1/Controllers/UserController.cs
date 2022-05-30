using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MyDbContext myDbContext;

        public UserController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        // 查找表中所有数据
        [HttpGet("user/getall")]
        public IActionResult GetAll()
        {
            List<User> UserTable = myDbContext.User.ToList(); // 查出所有
            return Ok(UserTable);
        }

        //添加一个数据，传入一个不带ID的，ID是自动加的，不需要传值，默认为0就行
        [HttpPost("user/postone")]
        public IActionResult PostOne(User user)
        {
            var Parameter = myDbContext.User.FirstOrDefault(a => a.user_id == user.user_id);
            if (Parameter != null)
            {
                return BadRequest(new { conut = -1, msg = "添加失败，id重复" });
            }
            myDbContext.User.Add(user);  //添加一个
            myDbContext.SaveChanges();
            return Ok();
        }

        //修改数据,传入对象，找到对应id的数据实现更新
        [HttpPost("parameter/modifyone")]
        public IActionResult Modify(int user_id, string user_pwd)
        {
            var Parameter = myDbContext.User.FirstOrDefault(a => a.user_id == user_id);
            if (Parameter == null)
            {
                return BadRequest(new { conut = -1, msg = "修改失败，未找到数据" });
            }
            //修改数据
            // ParameterTable.Id = parameter.Id;
            Parameter.user_pwd = user_pwd;

            myDbContext.User.Update(Parameter);
            myDbContext.SaveChanges();
            return Ok();
        }

        //移除一个对象，根据id移除
        [HttpPost("parameter/Removeone")]
        public IActionResult Remove(User user)
        {
            var Parameter = myDbContext.User.FirstOrDefault(a => a.user_id == user.user_id);
            //修改数据

            if (Parameter == null)
            {
                return NotFound();
            }
            myDbContext.User.Remove(Parameter);
            myDbContext.SaveChanges();
            return Ok(user);
        }
    }
}
