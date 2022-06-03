using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public partial class User
    {
        [Key]
        [Column("user_id")]
        public int user_id { get; set; }

        [Column("userEmail")]
        public string user_email { get; set; } = null!;

        [Column("userPwd")]
        public string user_pwd { get; set; } = null!;

        [Column("userName")]
        public string userName { get; set; } = null!;

        [Column("userType")]
        public string userType { get; set; } = null!;
    }
}
