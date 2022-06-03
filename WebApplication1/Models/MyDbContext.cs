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
        public virtual DbSet<Commodity> Commodity { get; set; } = null!;
        public virtual DbSet<Order> Order { get; set; } = null!;
        public virtual DbSet<TemporaryOrder> TemporaryOrder { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //映射到表Product，user_id作为主键
            modelBuilder.Entity<User>().ToTable("User").HasKey(p => new { p.user_id });
            //user_id映射到数据表中的列名是Id
            modelBuilder.Entity<User>().Property(p => p.user_id).HasColumnName("user_id");
            OnModelCreatingPartial(modelBuilder);

            
            modelBuilder.Entity<Commodity>().ToTable("Commodity").HasKey(p => new { p.commodity_id });
            
            modelBuilder.Entity<Commodity>().Property(p => p.commodity_id).HasColumnName("commodity_id");
            OnModelCreatingPartial(modelBuilder); 

            
            modelBuilder.Entity<Order>().ToTable("Order").HasKey(p => new { p.order_id });
            
            modelBuilder.Entity<Order>().Property(p => p.order_id).HasColumnName("order_id");
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<TemporaryOrder>().ToTable("TemporaryOrder").HasKey(p => new { p.temporaryOrder_id });

            modelBuilder.Entity<TemporaryOrder>().Property(p => p.temporaryOrder_id).HasColumnName("temporaryOrder_id");
            OnModelCreatingPartial(modelBuilder);


            /*modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            //public int Id { get; set; }
            //public string UserEmial { get; set; } = null!;
            //public string PassWord { get; set; } = null!;


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(e => e.user_id)
                    .HasMaxLength(200)
                    .HasColumnName("user_id");
            });

            OnModelCreatingPartial(modelBuilder);*/
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
