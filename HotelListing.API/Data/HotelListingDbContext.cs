using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HotelListing.API.Data
{

    //kw - This is the "CONTRACT" between the application and the database.

    public class HotelListingDbContext : DbContext //iw - inherit from EntityFrameworkCore, DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {
            
        }

        //  kw - Let the application KNOW ABOUT the db tables.
        //A DbSet represents the collection of all entities in the context, or that can be queried from the database. Note: DbSet objects are created from a DbContext using the DbContext.Set method.
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        //kw - NOTE: This 'override'  - Tells EntityFramework that when creating
        //  the database, ALSO do these "custom" actions IN ADDITION to its regular stuff/actions.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //kw - when we're building the model, this is what I want you to do...Like a Delphi 'OnCreate()'
            //  ...start with some default "seed" data (manual code-entry here - fake data).
            //array of Countries (rows of data)
            //
            //AFTER this fake data is entered, do ANOTHER MIGRATION via Tools|Nuget Package Manager|Package Manager Console (PM>):
            //
            ////          PM> add-migration SeededCountriesAndHotels (migration name used to roll back the migration, if needed)
            /// followed by
            ///             PM> update-database
            modelBuilder.Entity <Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name= "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BA"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Islands",
                    ShortName = "CI"
                });

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    Rating = 4.5,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suites",
                    Address = "Georgetown",
                    Rating = 4.3,
                    CountryId = 3
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand Palladium",
                    Address = "Nassua",
                    Rating = 4,
                    CountryId = 2
                });

        }
    }
}
