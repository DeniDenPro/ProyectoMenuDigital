using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Entities
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        //Modelado de las tablas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(25);
                entity.Property(s => s.Description).HasMaxLength(255);
                entity.Property(s => s.Order).IsRequired();

                //Entradas
                entity.HasData(
                new Category { Id = 1, Name = "Entradas", Description = "Pequeñas porciones para abrir el apetito antes del plato principal.", Order = 1 },
                new Category { Id = 2, Name = "Ensaladas", Description = "Opciones frescas y livianas, ideales como acompañamiento o plato principal.", Order = 2 },
                new Category { Id = 3, Name = "Minutas", Description = "Platos rápidos y clásicos de bodegón: milanesas, tortillas, revueltos.", Order = 3 },
                new Category { Id = 4, Name = "Pastas", Description = "Variedad de pastas caseras y salsas tradicionales.", Order = 5 },
                new Category { Id = 5, Name = "Parrilla", Description = "Cortes de carne asados a la parrilla, servidos con guarniciones.", Order = 4 },
                new Category { Id = 6, Name = "Pizzas", Description = "Pizzas artesanales con masa casera y variedad de ingredientes.", Order = 7 },
                new Category { Id = 7, Name = "Sandwiches", Description = "Sandwiches y lomitos completos preparados al momento.", Order = 6 },
                new Category { Id = 8, Name = "Bebidas", Description = "Gaseosas, jugos, aguas y opciones sin alcohol.", Order = 8 },
                new Category { Id = 9, Name = "Cerveza Artesanal", Description = "Cervezas de producción artesanal, rubias, rojas y negras.", Order = 9 },
                new Category { Id = 10, Name = "Postres", Description = "Clásicos dulces caseros para cerrar la comida.", Order = 10 }
                );
            });

            //Dish
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.HasKey(s => s.DishId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(255);
                entity.Property(s => s.Price).HasPrecision(18, 2).IsRequired();
                entity.Property(s => s.CreateDate).IsRequired();
                entity.Property(s => s.UpdateDate).IsRequired();
                //Relacion con Category
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Dishes)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
                //Relacion con OrderItems
                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Dish)
                      .HasForeignKey(oi => oi.DishId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(s => s.OrderId);
                entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
                entity.Property(e => e.DeliveryTo).HasMaxLength(255);
                //entity.Property(s => s.Notes).HasColumnType("varchar(max)");
                entity.Property(s => s.Price).HasPrecision(18, 2).IsRequired();
                entity.Property(s => s.CreateDate).IsRequired();
                entity.Property(s => s.UpdateDate).IsRequired();

                //Relacion con DeliveryType
                entity.HasOne(e => e.DeliveryType)
                      .WithMany(d => d.Orders)
                      .HasForeignKey(e => e.DeliveryTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
                //Relacion con Status
                entity.HasOne(e => e.OverallStatus)
                      .WithMany(s => s.Orders)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
                //Relacion con OrderItems
                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(s => s.OrderItemId);
                entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();
                entity.Property(s => s.Quantity).IsRequired();
                entity.Property(s => s.CreateDate).IsRequired();

                //Relacion con Status
                entity.HasOne(e => e.Status)
                      .WithMany(s => s.OrderItems)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //Status
            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(25);
                entity.HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "In Progress" },
                new Status { Id = 3, Name = "Ready" },
                new Status { Id = 4, Name = "Delivery" },
                new Status { Id = 5, Name = "Closed" }
                );
            });

            //DeliveryType
            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasColumnName("nvarchar(25)");
                entity.HasData(
                new DeliveryType { Id = 1, Name = "Delivery" },
                new DeliveryType { Id = 2, Name = "Takeaway" },
                new DeliveryType { Id = 3, Name = "Dine-In" }
                );
            });
        }
    }
}
