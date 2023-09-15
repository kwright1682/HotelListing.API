using HotelListing.API.Contracts; //kw note:  The reference to 'Contracts' refers to the INTERFACE definition in the 'Contracts' folder
using HotelListing.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    //Whenever you create a Repository, it MUST be REGISTERED in program.cs - see builder.Services.AddScoped()

    //This class INHERITS from IGenericRepository
    //  This is the IMPLEMENTATION of IGenericRepository.
    //  That's why we put it HERE, in the 'Repository' folder (ie, the Repository folder contains the IMPLEMENTATIONS)

    //GenericRepository is of type 'T'.
    //  Since 'T' is a generic, that means we don't KNOW what type 'T' will be at runtime.
    //  It might be type Country.  Or, it might be type 'Hotel'....etc.
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;

        /// <summary>
        /// //////////////////////////////////////////////
        /// All the BASIC CRUD operations needed by an API
        //////////////////////////////////////////////////

        //CONSTRUCTOR with db context being "injected"
        public GenericRepository(HotelListingDbContext thisContext)
        {
            this._context = thisContext; //We need this db context in order to implement the db stuff below.
        }
    
    

        public async Task<T> AddAsync(T entity)
        {
            //NOTE: Originally in the Controller, we specified private var _context.Countries....ie, specific to Country
            //  HERE, we use GENERIC type T.  So, the 'entity' being added will be of type 'T'...whatever type that
            //  may be at runtime....ie, passed in as param.
            await this._context.AddAsync(entity);
            await this._context.SaveChangesAsync();
            return entity; //return the entity (of type 'T') that was created - Hence the "Task" returning <T>
        }

        public async Task DeleteAsync(int id)
        {
            //First thing to do is find it
            var thisId = await GetAsync(id);
            this._context.Set<T>().Remove(thisId); //NOTE: Remove() cannot happen asynchronously. BUT this method IS called asynchronously.
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var thisId = await GetAsync(id); //Get the record with this 'id'

            if (thisId == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            //Go to the database, and retrieve the dataset associated with 'T' 
            return await this._context.Set<T>().ToListAsync(); //Return a list of whatever 'T' is.

            //NOTE: 'Set<T>()'  is DbSet just like in HotelListingDbContext.cs - a  dataset
            //      public DbSet<Hotel> Hotels { get; set; }
            //      public DbSet<Country> Countries { get; set; }
        }

        //Get a single record by id
        //Returns a record of type 'T'
        public async Task<T> GetAsync(int? id)
        {
            if (id == null)
            {
                return null; //404
            }
            //else
            return await this._context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            this._context.Update(entity); //go to the 'context', and I want you to Update this entity - Update() cannot happen asynchronously
            await this._context.SaveChangesAsync(); //save the changes...ASYNCHRONOUSLY
        }
    }
}
