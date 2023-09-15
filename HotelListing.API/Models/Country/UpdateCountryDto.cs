using HotelListing.API.Models.Country;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.ModelsDto.Country
{
    //Once defined (above), we can USE this data type (Dto) INSIDE the Countries Controller.
    //  SO, in the PostCountry() method, REPLACE the original param 'PostCountry(Country country)' with
    //  this NEW CreateCountryDto.  Using this Dto in the CountriesController essentially HIDES the 'id'
    //  property entirely.  Since id is a primary key in the table which gets auto-inc'd anyway, this
    //  should still work, AND it will protect the API from OVERPOSTING ATTACK where malicious user
    //  could send in a zillion id's.......see Microsoft Link in CountriesController, POST.
    //Besides, in a case like this where a primary key is auto-inc'd, the user should not want/need to
    //  send it as a param.

    //With the code replacement in ContriesController.cs, SWAGGER will use THIS Dto instead of the
    //  original 'country' parameter, thereby excluding 'id' from the input definition.

    public class UpdateCountryDto : BaseCountryDto
    {
        [Required]
        public int Id { get; set; }
    }
}
