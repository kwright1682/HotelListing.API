using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    //--------------------------

    public interface ICountriesRepository : IGenericRepository<Country>
    {
        //Via inheritance, this interface will automatically contain EVERYTHING in the IGenericRepository interface.
        //  PLUS it can ALSO have methods to handle any specific operations related to Country (this is just class inheritance)

        Task<Country> GetDetails(int id);
    }

}
