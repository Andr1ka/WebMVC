namespace WebMVC.Model.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Reflection.Emit;
    using WebMVC.Model;

    public class ApplicationContext : DbContext
        {
            //readonly StreamWriter logStream = new StreamWriter("mylog.txt", true);

            public DbSet<Customer> Customers { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<OrderProduct> OrderProducts { get; set; }

            public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
            {
                Database.EnsureCreated(); // может вызвать ошибки
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Настройка связи многие ко многим между Order и Product
                modelBuilder.Entity<OrderProduct>()
                    .HasKey(op => new { op.OrderId, op.ProductId });

                modelBuilder.Entity<OrderProduct>()
                    .HasOne(op => op.Order)
                    .WithMany(o => o.OrderProducts)
                    .HasForeignKey(op => op.OrderId);

                modelBuilder.Entity<OrderProduct>()
                    .HasOne(op => op.Product)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(op => op.ProductId);


                // Настройка связи один ко многим между Customer и Order
                modelBuilder.Entity<Order>()
                    .HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId);
            }
            //public override void Dispose()
            //{
            //    base.Dispose();
            //    logStream.Dispose();
            //}

            //public override async ValueTask DisposeAsync()
            //{
            //    await base.DisposeAsync();
            //    await logStream.DisposeAsync();
            //}
        }

}
