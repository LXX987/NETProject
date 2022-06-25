using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using WebApplication1.DLLs;

namespace WebApplication1.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        MyDbContext myDbContext;

        public CommodityController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        // 查找表中所有数据
        [HttpGet("commodity/getall")]
        public IActionResult GetAll()
        {
            List<Commodity> CommodityTable = myDbContext.Commodity.ToList(); // 查出所有
            return Ok(CommodityTable);
        }

        // 插入商品数据，传入一个不带ID的，ID是自动加的，不需要传值，默认为0就行
        [HttpPost("commodity/insertOne")]
        public IActionResult InsertOne(int commodity_id, int user_id, string commodity_name, int item_price, int total_count, DateTime start_time, DateTime end_time)
        {
            var Parameter = myDbContext.Commodity.FirstOrDefault(a => a.commodity_id == commodity_id);
            if (Parameter != null)
            {
                return BadRequest(new { conut = -1, msg = "添加失败，id重复" });
            }

            Commodity commodity = new Commodity();
            commodity.commodity_id = commodity_id;
            commodity.user_id = user_id;
            commodity.commodity_name = commodity_name;
            commodity.item_price = item_price;
            commodity.total_count = total_count;
            commodity.start_time = start_time;
            commodity.end_time = end_time;
            myDbContext.Commodity.Add(commodity);  //添加一个
            myDbContext.SaveChanges();
            return Ok();
        }

        // 删除商品接口
        [HttpPost("commodity/deleteOne")]
        public IActionResult DeleteOne(Commodity commodity)
        {
            var Parameter = myDbContext.Commodity.FirstOrDefault(a => a.commodity_id == commodity.commodity_id);

            //修改数据
            if (Parameter == null)
            {
                return NotFound();
            }

            myDbContext.Commodity.Remove(Parameter);
            myDbContext.SaveChanges();
            return Ok(commodity);
        }

        // 修改商品信息，找到对应id的数据实现更新
        [HttpPost("commodity/modifyOne")]
        public IActionResult Modify(int commodity_id, string commodity_name, int item_price, int total_count, DateTime start_time, DateTime end_time)
        {
            var Parameter = myDbContext.Commodity.FirstOrDefault(a => a.commodity_id == commodity_id);
            if (Parameter == null)
            {
                return BadRequest(new { conut = -1, msg = "修改失败，未找到数据" });
            }
            //修改数据
            // ParameterTable.Id = parameter.Id;
            Parameter.commodity_name = commodity_name;
            Parameter.item_price = item_price;
            Parameter.total_count = total_count;
            Parameter.start_time = start_time;
            Parameter.end_time = end_time;

            myDbContext.Commodity.Update(Parameter);
            myDbContext.SaveChanges();
            return Ok();
        }

        // 购买商品
        [HttpPost("commodity/purchase")]
        [Authorize]
        public async Task<IActionResult> purchase(Commodity commodity)
        {
            //注，形参虽然是commodity，但是实际是因为需要给到的商品名称和购买数量，所以选择形参为这个实体类
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);
            Console.WriteLine(user_id);
            // 查找商品
            var Parameter = await TaskCaller(commodity);
            if (Parameter == null)
            {
                return BadRequest(new { conut = -1, msg = "购买失败，商品不存在" });
            }
            if (Parameter.total_count < commodity.total_count)
            {
                // 库存不足
                return BadRequest(new { conut = -1, msg = "购买失败，商品不足" });
            }
            Parameter.total_count = Parameter.total_count - commodity.total_count;
            // 创建订单
            TemporaryOrder temporaryOrder = new TemporaryOrder();
            temporaryOrder.user_id = user_id;
            temporaryOrder.commodity_id = Parameter.commodity_id;
            temporaryOrder.time = DateTime.Now;
            temporaryOrder.count = commodity.total_count;
            temporaryOrder.total_prince = Parameter.item_price * commodity.total_count;
            myDbContext.TemporaryOrder.Add(temporaryOrder);  //添加一个
            myDbContext.Commodity.Update(Parameter); // 更新商品属性
            myDbContext.SaveChanges();
            return Ok(temporaryOrder);

        }

        private async Task<Commodity> TaskCaller(Commodity commodity)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var result = await myDbContext.Set<Commodity>().FirstOrDefaultAsync(a => a.commodity_name == commodity.commodity_name);
            return result;

        }

        [HttpPost("commodity/confirm")]
        public async Task<IActionResult> confirm()
        {
            Random rd = new Random();
            int id = rd.Next();
            COMdll cOMdll = new COMdll();
            cOMdll.comOrderdll(id);
            return Ok();
        }

            // 搜索商品
            [HttpPost("commodity/searchCommodity")]
        public async Task<IActionResult> searchCommodity(Commodity commodity)
        {
            // 从commodity里面拿到商品名称
            string commodity_name = commodity.commodity_name;
            // 查找商品
            var Parameter = await TaskSearchCommodity(commodity_name);
            return Ok(Parameter);
        }
        private async Task<IQueryable<Commodity>> TaskSearchCommodity(string commodity_name)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var query = from d in myDbContext.Commodity
                        where d.commodity_name == commodity_name
                        select d;
            return query;

        }

        // 发布一项商品
        [HttpPost("commodity/postCommodity")]
        [Authorize]
        public async Task<IActionResult> postCommodity(Commodity commodity)
        {
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);
            // 创建新商品
            commodity.user_id = user_id;
            myDbContext.Commodity.Add(commodity);  //添加一个
            myDbContext.SaveChanges();
            return Ok(commodity);
        }

        // 团长查看自己的全部商品
        [HttpPost("commodity/findMyAllCommodities")]
        [Authorize]
        public async Task<IActionResult> findMyAllCommodities()
        {
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);

            var Parameter = await TaskfindMyAllCommodities(user_id);
            return Ok(Parameter);

        }

        private async Task<IQueryable<Commodity>> TaskfindMyAllCommodities(int user_id)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var query = from d in myDbContext.Commodity
                        where d.user_id == user_id
                        select d;
            return query;
        }

        // 团长搜索一个自己的商品
        [HttpPost("commodity/findMyOneCommodities")]
        [Authorize]
        public async Task<IActionResult> findMyOneCommodities(Commodity commodity)
        {
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);

            string commodity_name = commodity.commodity_name;

            var Parameter = await TaskfindMyOneCommodities(user_id, commodity_name);
            return Ok(Parameter);

        }

        private async Task<IQueryable<Commodity>> TaskfindMyOneCommodities(int user_id, string commodity_name)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var query = from d in myDbContext.Commodity
                        where d.commodity_name == commodity_name && d.user_id == user_id
                        select d;
            return query;
        }


        // 团长修改自己的商品信息
        [HttpPost("commodity/modifyInformation")]
        [Authorize]
        public async Task<IActionResult> modifyInformation(Commodity commodity)
        {
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);
            // 修改商品信息
            commodity.user_id = user_id;
            // 检查时间是否过时
            int compareResult = commodity.end_time.CompareTo(DateTime.Now);
            Console.WriteLine(compareResult);// 如果修改后的时间大于现在的时间，就是1
            // 订单信息不用修改，因为商品id不会变
            if (compareResult == 1)
            {
                myDbContext.Commodity.Update(commodity);
                myDbContext.SaveChanges();
                return Ok(commodity);
            }
            else
            {
                return BadRequest(new { conut = -1, msg = "修改失败，商品已下架" });
            }

        }

        // 查看所有正在进行当中的商品信息
        [HttpPost("commodity/getAllBuying")]
        public async Task<IActionResult> getAllBuying()
        {
            DateTime currentTime = DateTime.Now;
            var Parameter = await TaskgetAllBuying(currentTime);
            return Ok(Parameter);
        }

        private async Task<IQueryable<Commodity>> TaskgetAllBuying(DateTime currentTime)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var query = from d in myDbContext.Commodity
                        where d.end_time.CompareTo(currentTime) == 1
                        select d;
            return query;
        }
    }
}
