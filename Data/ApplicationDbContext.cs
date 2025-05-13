using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STGeorgeReservation.Models;
using TheProfessionals.Models;


namespace STGeorgeReservation.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, aspnetroles, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        public DbSet<aspnetroles> Roles { get; set; }
        public DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
        public DbSet<Buildings> Buildings { get; set; }
        public DbSet<Floors> Floors { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Permissions_lkp> Permissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=STGeorgeDB;user=root;password=654321;",
        //    ServerVersion.AutoDetect("server=127.0.0.1;port=3306;database=STGeorgeDB;user=root;password=654321;"));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API Configurations
            modelBuilder.Entity<Buildings>().HasMany(b => b.Floors).WithOne(f => f.Building).HasForeignKey(f => f.BuildingId);
            modelBuilder.Entity<Floors>().HasMany(f => f.Rooms).WithOne(r => r.Floor).HasForeignKey(r => r.FloorId);
            modelBuilder.Entity<Rooms>().HasMany(r => r.Reservations).WithOne(res => res.Room).HasForeignKey(res => res.RoomId);
        }

        
    }
}


