using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Data
{
    public class Commodity
    {
        public int commodity_id { get; set; }

        public int user_id { get; set; }

        public string commodity_name { get; set; } = null!;

        public int item_price { get; set; }

        public int total_count { get; set; }

        public DateTime start_time { get; set; }

        public DateTime end_time { get; set; }
    }
}
