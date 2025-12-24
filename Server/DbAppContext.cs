using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DbAppContext : DbContext
    {
        public DbSet<Parthners> Parthners { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Parthners;Username=postgres;Password=C0d38_50AdM1Nn6");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parthners>().ToTable("Parthners");
            modelBuilder.Entity<ProductType>().ToTable("ProductType");
            modelBuilder.Entity<Products>().ToTable("Products");
            modelBuilder.Entity<Material>().ToTable("Material");


            modelBuilder.Entity<Products>()
                .HasOne(p => p.Parthner)
                .WithMany(b => b.ProductEntities)
                .HasForeignKey(p => p.ParthnerId);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.ProductType)
                .WithMany(b => b.ProductEntities)
                .HasForeignKey(p => p.TypeId);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Material)
                .WithMany(b => b.ProductEntities)
                .HasForeignKey(p => p.MaterialId);
        }


        //        Add-Migration Initial
        //такая команда нужна для миграции с помощью entity framework в бд

        //Чтобы обновить бд нужно ввести Update-Database

        //это все вводить в пакетный менеджер в вс
    }
}
