using HotelListing.API.Models.Country;

namespace HotelListing.API.ModelsDto.Country
{
    //The WHOLE POINT of Dto's is that we do NOT want User to interact directly with Data Objects (classes).
    //Dto gives us a way to "control" which fields are accessible via API.
    //This is IMPORTANT when considering things such as "over-posting" attacks.
    //
    //We then use a "mapper", to map the fields of a given Dto INTO it's Data Object (class) counterpart.
    //
    //A Dto (data transfer object) is an "ABSTRACTION", and ONLY INCL's the data that we DO want to transfer.
    //Same as with our Country class which is a 'model', a Dto is ALSO a model.
    //
    //But, the idea is to CONTROL data/params passed into an API.  If we use the Country class, then
    //  ALL of it's fields, including 'id' are available (and visible in SWAGGER). 
    //  We DO NOT want 'id' to be visible/usable.  SO, we create a Dto "model" to take the place
    //  of our Country class, in the API code.  In the Dto, we can LIMIT which params are visible/available
    //  to a user/caller.
    //
    //This Dto (class) is responsible for handling creation requests for Country.
    //
    //This Dto Only accepts 'bindings' for Name and ShortName, but NOT for id (as def'd in the Country class)
    //
    //  We DO NOT want to include 'id'.
    //  We ONLY want to include Name and ShortName.

    //The Original Country class in Country.cs has an 'id' property, which we DO NOT want to map into this Dto.
    //Btw, ANY fields that a malicious user might try to use, that are not BOUND, will be ignored and produce an http error.
    public class CreateCountryDto : BaseCountryDto
    {
        //[Required] //Add annotation 'Required' if a field is required
        //public string Name { get; set; } //commented - NOW INHERITING from BaseCountryDto

        //No 'Required' annotation, so this one is NOT REQUIRED
        //public string ShortName { get; set; }  //commented - NOW INHERITING from BaseCountryDto
    }
}
