using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    //已支付订单
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int order_id { get; set; }

        [ForeignKey("user_id")]
        public int user_id { get; set; }
        public virtual User User { get; set; } = new User();

        [ForeignKey("commodity_id")]
        public int commodity_id { get; set; }
        public virtual Commodity Commodity { get; set; } = new Commodity();

        [Column("time")]
        public DateTime time { get; set; }

        [Column("count")]
        public int count { get; set; }

        [Column("total_price")]
        public int total_prince { get; set; }
    }
}
