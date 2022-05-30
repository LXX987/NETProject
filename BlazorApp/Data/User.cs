using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Data
{
    public class User
    {
        public int user_id { get; set; }

        public string user_email { get; set; } = null!;

        public string user_pwd { get; set; } = null!;
    }
}
