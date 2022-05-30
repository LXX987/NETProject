using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebApplication1.Models
{
    public partial class MyDbContext : DbContext
    {
        
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        public virtual DbSet<User> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            //public int Id { get; set; }
            //public string UserEmial { get; set; } = null!;
            //public string PassWord { get; set; } = null!;


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("userTable");
                entity.Property(e => e.user_id)
                    .HasMaxLength(200)
                    .HasColumnName("user_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
