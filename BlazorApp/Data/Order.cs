using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Data
{
    public class TemporaryOrder
    {
        public int temporaryOrder_id { get; set; }

        public int user_id { get; set; }

        public int commodity_id { get; set; }

        public DateTime time { get; set; }

        public int count { get; set; }

        public int total_prince { get; set; }

    }
}
