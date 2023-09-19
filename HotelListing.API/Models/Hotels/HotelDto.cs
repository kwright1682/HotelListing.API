using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HotelListing.API.Models.Hotels
{

    public class HotelDto : BaseHotelDto
    {
        public int Id { get; set; }

        //public string Name { get; set; } 
        //public string Address { get; set; }   //<------------These fields were MOVED TO BaseHotelDto
        //public double Rating { get; set; }
        //public int CountryId { get; set; } 

    }


}
