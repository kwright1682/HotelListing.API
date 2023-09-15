using Serilog.Sinks.SystemConsole.Themes;

namespace HotelListing.API.Contracts
{
    public interface IGenericRepository<T> where T : class //ie, where 'T' represents a Class
    {
        //This is all the basic CRUD stuff - Create, Read, Update, Delete
        
        //An Interface is a CONTRACT (which is why this folder is named 'Contracts')
        //
        //Here, we will specify HOW to communicate with the db on our behalf.
        //The Controller does not know or care HOW this is done.
        //  It only knows that IGenericRepository is in charge of that
        //
        //SO, the overarching purpose of THIS code is 'separation of concerns'
        //  Ie, get all the direct db access OUT OF THE Controller.


        //Get ONE record, and return it as type 'T'
        //An asynchronous task that recieves some type 'T', and takes a nullable parm 'id'
        Task<T> GetAsync(int? id);

        //An asynchronous task that gets ALL records, and returns them as a List of type 'T's
        Task<List<T>> GetAllAsync();

        //An asynchronous task that ADDs a type 'T' record/entity, and returns it as type 'T'
        Task<T> AddAsync(T entity); //Takes some generically named "entity"...we don't know if the entity will be a Hotel, a Country...etc.

        //An asynchronous task that DELETEs a record 
        Task DeleteAsync(int id);

        //An asynchronous task that UPDATEs a record
        Task UpdateAsync(T entity);

        //An asynchronous task that tests whether on not a Country Exists.
        //  This REPLACES the original CountryExists() bool function in the controller, because we DO NOT
        //  want the Controller to be referencing the db context (ie, _context) directly.
        //  So move it HERE instead, and get it OUT OF THE Controller.
        Task<bool> Exists(int id); //Return a boolean
    }

}
