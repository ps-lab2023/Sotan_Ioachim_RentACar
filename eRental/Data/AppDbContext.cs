using eRental.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace eRental.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rental>()
                .HasMany(r => r.Vehicles)
                .WithMany(v => v.Rentals)
                .UsingEntity<Dictionary<string, object>>(
                    "RentalVehicle",
                    j => j.HasOne<Vehicle>().WithMany().HasForeignKey("RentalId"),
                    j => j.HasOne<Rental>().WithMany().HasForeignKey("VehicleId"));

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.Categories)
                .WithMany(c => c.Vehicles)
                .UsingEntity<Dictionary<string, object>>(
                    "VehicleCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("VehicleId"),
                    j => j.HasOne<Vehicle>().WithMany().HasForeignKey("CategoryId"));
        }
    }
}
