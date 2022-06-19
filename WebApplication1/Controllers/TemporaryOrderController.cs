using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

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

    }
}
