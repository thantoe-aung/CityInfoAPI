using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.DbContexts
{
    public class CityInfoContext : DbContext
    {

        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointOfInterest { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CityInfo.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York")
                {
                    Id = 1,
                    Description = "New York"
                },
                new City("Bangkok")
                {
                    Id = 2,
                    Description = "Bangkok"
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "PSG"
                }

                );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id= 1,
                    CityId = 1,
                    Description = "Park"
                },
                 new PointOfInterest("Bridge")
                 {
                     Id = 2,
                     CityId = 2,
                     Description = "Drugs"
                 },
                  new PointOfInterest("Tower")
                  {
                      Id = 3,
                      CityId = 3,
                      Description = "Tower"
                  }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
