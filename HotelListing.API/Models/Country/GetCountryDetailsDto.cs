using HotelListing.API.Models.Hotels;

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
    public class GetCountryDetailsDto 
    {
        public int Id { get; set; } //We might want 'id' available to User for such things as e.g. editing Name or ShortName
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<HotelDto> Hotels { get; set; } //Map BACK to Hotels in Country class !!
    }

}
