using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.Controllers
{
    // 实现临时订单
    [Route("api/")]
    [ApiController]
    public class TemporaryOrderController : ControllerBase
    {
        MyDbContext myDbContext;

        public TemporaryOrderController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        // 查找表中所有数据
        [HttpGet("temporaryOrder/getall")]
        public IActionResult GetAll()
        {
            List<TemporaryOrder> TemporaryOrderTable = myDbContext.TemporaryOrder.ToList(); // 查出所有
            return Ok(TemporaryOrderTable);
        }

        // 插入临时订单数据，传入一个不带ID的，ID是自动加的，不需要传值，默认为0就行
        [HttpGet("temportaryOrder/insertOne")]
        public async Task<IActionResult> PostOne(int temporaryOrder_id, int user_id, int commodity_id, DateTime time, int count, int total_prince)
        {
            //当前线程
            var info = string.Format("api执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(info);
            var Parameter = await TaskCaller(temporaryOrder_id);
            if (Parameter != null)
            {
                return BadRequest(new { conut = -1, msg = "添加失败，id重复" });
            }
            TemporaryOrder temporaryOrder = new TemporaryOrder();
            temporaryOrder.temporaryOrder_id = temporaryOrder_id;
            temporaryOrder.user_id = user_id;
            temporaryOrder.commodity_id = commodity_id;
            temporaryOrder.time = time;
            temporaryOrder.count = count;
            temporaryOrder.total_prince = total_prince;
            myDbContext.TemporaryOrder.Add(temporaryOrder);  //添加一个
            myDbContext.SaveChanges();
            return Ok();
        }

        private async Task<TemporaryOrder> TaskCaller(int temporaryOrder_id)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var result = await myDbContext.Set<TemporaryOrder>().FirstOrDefaultAsync(a => a.temporaryOrder_id == temporaryOrder_id);
            return result;

        }

        // 查看某个用户的订单
        [HttpPost("temportaryOrder/searchOne")]
        [Authorize]
        public async Task<List<TemporaryOrder>> searchOne()
        {
            // 获取token user_id
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);

            // 查看该user_id的全部记录
            var Parameter = await TaskSearchUser(user_id);
            return Parameter;
        }

        private async Task<List<TemporaryOrder>> TaskSearchUser(int user_id)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var query = from d in myDbContext.TemporaryOrder
                        where d.user_id == user_id
                        select d;
            List<TemporaryOrder> result = await query.ToListAsync();
            
            return result;
        }

        // 搜索一条记录
        [HttpPost("temportaryOrder/searchOneRecord")]
        [Authorize]
        public async Task<IActionResult> searchOneRecord(Commodity commodity)
        {
            // 获取token user_id
            var token = HttpContext.GetTokenAsync("Bearer", "access_token");
            string jwtStr = token.Result;
            /*string jwtStr = Request.Headers["Authorization"];//Header中的token*/
            var tm = JwtHelper.SerializeJwt(jwtStr);
            string id = tm.Uid;
            int user_id = 0;
            int.TryParse(id, out user_id);

            string commodity_name = commodity.commodity_name;
            // 搜索商品名称对应的id，可能有好多个id，list
            var Parameter = await TaskSearchCommodityId(commodity_name);
            // 用商品的id去找订单
            var orderList = await TaskSearchOrderList(user_id, Parameter.commodity_id);
            return Ok(new { Parameter, orderList });
        }

        private async Task<Commodity> TaskSearchCommodityId(string commodity_name)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var result = await myDbContext.Set<Commodity>().FirstOrDefaultAsync(a => a.commodity_name == commodity_name);
            return result;
        }

        private async Task<IQueryable<TemporaryOrder>> TaskSearchOrderList(int user_id, int commodity_id)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var query = from d in myDbContext.TemporaryOrder
                        where d.commodity_id == commodity_id && d.user_id == user_id
                        select d;
            return query;

        }

        // 点击查看一条记录的详细信息
        [HttpPost("temportaryOrder/findInformation")]
        public async Task<IActionResult> findInformation(TemporaryOrder temporaryOrder)
        {
            // 需要商品id
            var CommodityItem = await TaskFindCommodity(temporaryOrder.commodity_id);
            return Ok(new { CommodityItem });
        }

        private async Task<Commodity> TaskFindCommodity(int commodity_id)
        {
            // return string.Format("task 执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
            var result = await myDbContext.Set<Commodity>().FirstOrDefaultAsync(a => a.commodity_id == commodity_id);
            return result;
        }
    }
}
