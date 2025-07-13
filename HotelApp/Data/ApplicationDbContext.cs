using System.Reflection.Emit;
using HotelApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            base.OnModelCreating(builder);

            builder.Entity<Hotelincludes>()
                .HasKey(h => new { h.HotelId, h.IncludeId });
        }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<HotelPhoto> hotelPhotos { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<Include> includes { get; set; }
        public DbSet<Review> reviews { get; set; }

    }
}
