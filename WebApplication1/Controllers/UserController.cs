using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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
        [HttpPost("user/insertOne")]
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
        [HttpPost("user/modifyOne")]
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
        [HttpPost("user/removeOne")]
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

        //用户登录
        [HttpPost("user/login")]
        public IActionResult Login(User user)
        {
            string user_email = user.user_email;
            string user_pwd = user.user_pwd;
            string userType = user.userType;
            var Parameter = myDbContext.User.FirstOrDefault(a => a.user_email == user_email);
            if (Parameter == null)
            {
                return BadRequest(new { conut = -1, msg = "该邮箱未注册账号，未找到数据" });
            }
            if (user_pwd != Parameter.user_pwd)
            {
                return BadRequest(new { conut = -1, msg = "密码错误" });
            }
            else if (userType != Parameter.userType)
            {
                return BadRequest(new { conut = -1, msg = "用户类型错误" });
            }
            else
            {
                string jwtStr = string.Empty;
                // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
                TokenModel tokenModel = new TokenModel { Uid = Parameter.user_id.ToString() };
                jwtStr = JwtHelper.IssueJwt(tokenModel);//获取到一定规则的 Token 令牌

                return Ok(new { data = Parameter, msg = "登录成功" , token= jwtStr });

            }
        }

        //获取一个用户的数据
        [HttpGet("user/searchUser")]
        [Authorize]
        public IActionResult SearchUser()
        {
            var token=HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);
            var Parameter = myDbContext.User.FirstOrDefault(a => a.user_id == user_id);
            return Ok(Parameter);
        }
    }
}
