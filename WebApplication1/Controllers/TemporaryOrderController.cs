using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        
    }
}
