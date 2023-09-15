using HotelListing.API.Models.Hotels;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Models.Country

////////////// IMPORTANT NOTE ///////////////////////
///
/// RULE OF THUMB:
/// -------------
/// Dto's should NEVER have a field that is DIRECTLY RELATED to a data-model (class).
/// Ie, DTO's ONLY TALK TO other DTO's
/// 
/// The ONLY TIME a Dto will "cross paths" with a data-model is when
/// we are doing a MAPPING operation.
/// 
/////////////////////////////////////////////////////
{
    //This is just a MODEL of a 'real' Country (ie, a Country CLASS)
    public class GetCountryDto : BaseCountryDto
    {
        //A Dto allows you to control exactly WHAT gets returned when a given ENDPOINT is called.
        //In THIS example, we DO NOT wish to return a list of Hotels along with the Countries.
        //SO, we simply don't include it.  We create this Dto instead, and use IT in the GET instead of
        //  using the Country class.
        //NOTE: Same with the other fields.  Include/exclude them as needed to fit what you wish to return.

        public int Id { get; set; } //We might want 'id' available to User for such things as e.g. editing Name or ShortName
        //public string Name { get; set; }      //commented - NOW INHERITING from BaseCountryDto
        //public string ShortName { get; set; } //commented - NOW INHERITING from BaseCountryDto

        //NOTE:
        //This is suspiciously similar to the Country class.
        //HOWEVER, this new dto class DOES NOT CONTAIN a list of Hotels as does Country class.
        //     i.e.,          public virtual IList<Hotel> Hotels { get; set; }

        public List<HotelDto> Hotels { get; set; }
    }

}
