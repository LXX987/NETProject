using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Commodity
    {
        [Key]
        [Column("commodity_id")]
        public int commodity_id { get; set; }

        [ForeignKey("user_id")]
        public int user_id { get; set; }
        public virtual User Users { get; set; } = new User();

        [Column("commodity_name")]
        public string commodity_name { get; set; } = null!;

        [Column("item_price")]
        public int item_price { get; set; } = 0!;

        [Column("total_count")]
        public int total_count { get; set; } = 0!;

        [Column("start_time")]
        public DateTime start_time { get; set; } = DateTime.Now;

        [Column("end_time")]
        public DateTime end_time { get; set; } = DateTime.Now;
    }
}
