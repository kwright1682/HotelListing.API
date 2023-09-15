using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    //Whenever you create a Repository, it MUST be REGISTERED in program.cs - see builder.Services.AddScoped()

    //////////////////////////////////////////////////////////
    //INHERIT FROM BOTH GenericRepository AND ICountriesRepository
    //
    //EVERY Interface ("contract") needs an implementation. THIS IS the implementation of ICountriesRepository
    //////////////////////////////////////////////////////////
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository //ICountriesRepository inherits from GenericRepository
    {
        private readonly HotelListingDbContext _context;

        public CountriesRepository(HotelListingDbContext thisContext) : base(thisContext) //Take a copy of the context (via param), and pass it down to the base-class.
        {
            this._context = thisContext;
        }

        public async Task<Country> GetDetails(int id) // ? means nullable
        {
            //Include() all the Hotels, and then FirstOrDefaultAsync() will return NULL (which is the meaning of 'Default' in the method name) if cannot find it.
            //Return it if found.
            return await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(thisId => thisId.Id == id);
        }

    }
}
