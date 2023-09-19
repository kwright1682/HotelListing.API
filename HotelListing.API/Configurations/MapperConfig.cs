using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotels;
using HotelListing.API.ModelsDto.Country;

namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile //Inherit from 'Profile' - Remember to add using statement for Profile
    {
        // Why do we need Mapping??
        //GOOD - https://dotnettutorials.net/lesson/automapper-in-c-sharp/

        //constructor
        public MapperConfig()
        {
            //Mapping-definitions are static, defined ONCE and re-used throughout the lifetime of the application.

            //This constructor allows us to "map" our data types.

            //We have a new Country type called CreateCountryDto. And, it would
            //be nice to have it MAPPED to the Country data type, so that we don't
            //have to do the manual conversion in CountriesController - var newCountry = new Country...etc
            //  because, while in this example program it's only two fields (Name and ShortName), it COULD HAVE been 20 or any other number of fields!!
            //So, MAPPING handles this in a much cleaner, faster and less error-prone way than doing it manually!

            //Create a "map" FROM the Country class TO the CreateCountryDto class
            //  but then add '.ReverseMap() to reverse the mapping to be FROM CreateCountryDto TO Country.
            //          CreateMap<From, To>
            //  With this, you can "map" in either direction...(I think...)
            //
            CreateMap<Country, CreateCountryDto>().ReverseMap(); //Q: WHY NOT JUST REVERSE THE PARAMS IN THE FIRST PLACE, and not even use ReverseMap() ????
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDetailsDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();


            //IN GENERAL: You need to create a 'mapping' for EACH and EVERY Dto!!!  

            //NOTE: You will need to go to Program.cs and REGISTER this automapper configuration -- builder.Services.AddAutoMapper()...
        }
    }
}
