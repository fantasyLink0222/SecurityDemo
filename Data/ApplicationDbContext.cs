using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecurityDemo.Models;

namespace SecurityDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Building>()
                .HasKey(b => b.buildingId);

            modelBuilder.Entity<Room>()
                .HasKey(r => r.roomId);

            modelBuilder.Entity<City>()
                .HasMany(c => c.buildings)
                .WithOne(b => b.city)
                .HasForeignKey(b => b.cityId);

            modelBuilder.Entity<City>()
                .HasKey(b => b.cityId);

            modelBuilder.Entity<Building>()
                .HasOne(b => b.city)
                .WithMany(c => c.buildings)
                .HasForeignKey(b => b.cityId);

            // Seed the database
            var adminRoleId = "admin-role-id"; // Create a Admin role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
            var customerRoleId = "customer-role-id"; // Create a Customer role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = customerRoleId,
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            });

            var userId = "admin-user-id"; // Create a user
            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = userId,
                UserName = "admin@home.com",
                NormalizedUserName = "ADMIN@HOME.COM",
                Email = "admin@home.com",
                NormalizedEmail = "ADMIN@HOME.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssw0rd!"),
                SecurityStamp = string.Empty
            });

            // Assign the user to the role
            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasData(new IdentityUserRole<string>
                        {
                            RoleId = adminRoleId,
                            UserId = userId
                        });

            userId = "mateo-user-id"; // Create a user
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = userId,
                UserName = "mateo@gmail.com",
                NormalizedUserName = "MATEO@GMAIL.COM",
                Email = "mateo@gmail.com",
                NormalizedEmail = "MATEO@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssw0rd!"),
                SecurityStamp = string.Empty
            });

            // Assign the user to the role
            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasData(new IdentityUserRole<string>
                        {
                            RoleId = customerRoleId,
                            UserId = userId
                        });

            userId = "priya-user-id"; // Create a user
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = userId,
                UserName = "priya@yahoo.com",
                NormalizedUserName = "PRAYA@YAHOO.COM",
                Email = "priya@yahoo.com",
                NormalizedEmail = "PRAYA@YAHOO.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssw0rd!"),
                SecurityStamp = string.Empty
            });

            // Assign the user to the role
            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasData(new IdentityUserRole<string>
                        {
                            RoleId = customerRoleId,
                            UserId = userId
                        });

            userId = "keiko-user-id"; // Create a user
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = userId,
                UserName = "keiko@Outlook.com",
                NormalizedUserName = "KEIKO@OUTLOOK.COM",
                Email = "keiko@Outlook.com",
                NormalizedEmail = "KEIKO@OUTLOOK.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssw0rd!"),
                SecurityStamp = string.Empty
            });

            // Assign the user to the role
            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasData(new IdentityUserRole<string>
                        {
                            RoleId = customerRoleId,
                            UserId = userId
                        });

            userId = "kwame-user-id"; // Create a user
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = userId,
                UserName = "kwame@aol.com",
                NormalizedUserName = "KWAME@AOL.COM",
                Email = "kwame@aol.com",
                NormalizedEmail = "KWAME@AOL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssw0rd!"),
                SecurityStamp = string.Empty
            });

            // Assign the user to the role
            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasData(new IdentityUserRole<string>
                        {
                            RoleId = customerRoleId,
                            UserId = userId
                        });

            modelBuilder.Entity<City>().HasData(    // Populate Cities
                new City { cityId = 1, cityName = "Vancouver" },
                new City { cityId = 2, cityName = "Toronto" },
                new City { cityId = 3, cityName = "Montreal" },
                new City { cityId = 4, cityName = "Calgary" },
                new City { cityId = 5, cityName = "Surrey" }
            );

            modelBuilder.Entity<Building>().HasData( // Populate Buildings
                new Building { buildingId = 1, name = "Building A", cityId = 1 },
                new Building { buildingId = 2, name = "Building B", cityId = 5 },
                new Building { buildingId = 3, name = "Building C", cityId = 5 },
                new Building { buildingId = 4, name = "Building D", cityId = 1 },
                new Building { buildingId = 5, name = "Building E", cityId = 4 },
                new Building { buildingId = 6, name = "Building F", cityId = 1 },
                new Building { buildingId = 7, name = "Building G", cityId = 4 },
                new Building { buildingId = 8, name = "Building H", cityId = 5 },
                new Building { buildingId = 9, name = "Building I", cityId = 1 },
                new Building { buildingId = 10, name = "Building J", cityId = 4 }
            );

            modelBuilder.Entity<Room>().HasData(    // Populate Rooms
                new Room { roomId = 1, name = "Room 101", capacity = 22, buildingId = 1 },
                new Room { roomId = 2, name = "Room 201", capacity = 35, buildingId = 1 },
                new Room { roomId = 3, name = "Room 301", capacity = 15, buildingId = 2 },
                new Room { roomId = 4, name = "Room 151", capacity = 55, buildingId = 3 },
                new Room { roomId = 5, name = "Room 251", capacity = 55, buildingId = 3 },
                new Room { roomId = 6, name = "Room 301", capacity = 25, buildingId = 3 },
                new Room { roomId = 7, name = "Room 101", capacity = 12, buildingId = 4 },
                new Room { roomId = 8, name = "Room 201", capacity = 75, buildingId = 4 },
                new Room { roomId = 9, name = "Room 301", capacity = 8, buildingId = 4 },
                new Room { roomId = 10, name = "Room 312", capacity = 21, buildingId = 4 },
                new Room { roomId = 11, name = "Room 313", capacity = 35, buildingId = 4 },
                new Room { roomId = 12, name = "Room 314", capacity = 77, buildingId = 4 },
                new Room { roomId = 13, name = "Room 401", capacity = 75, buildingId = 5 },
                new Room { roomId = 14, name = "Room 801", capacity = 30, buildingId = 6 },
                new Room { roomId = 15, name = "Room 901", capacity = 28, buildingId = 7 },
                new Room { roomId = 16, name = "Room 551", capacity = 55, buildingId = 8 },
                new Room { roomId = 17, name = "Room 801", capacity = 30, buildingId = 9 },
                new Room { roomId = 18, name = "Room 601", capacity = 25, buildingId = 10 },
                new Room { roomId = 19, name = "Room 701", capacity = 21, buildingId = 10 },
                new Room { roomId = 20, name = "Room 801", capacity = 30, buildingId = 10 },
                new Room { roomId = 21, name = "Room 901", capacity = 20, buildingId = 10 }
            );
        }
    }
}