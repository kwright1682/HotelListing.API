using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Hotels
{
    //ABSTRACT classes cannot be instantiated.
    //  It is designed to be INHERITED by subclasses that EITHER implement or override its methods.
    //  In other words, abstract classes are either partially implemented or not implemented at all.
    public abstract class BaseHotelDto
    {
        //NOTE:
        //There are a number of VALIDATION types/rules available.
        //Apply the one's you want, PER FIELD

        [Required] //validations
        public string Name { get; set; }

        [Required] //validations
        public string Address { get; set; }

        //NOT REQUIRED - No validations on this one
        public double? Rating { get; set; } //Made this one NULLABLE - If no value provided, then NULL will be used as the value

        [Required] //validation
        [Range(1, int.MaxValue)] //validation  - Range(minval,maxval)
        public int CountryId { get; set; } //Note: If the value is not a valid 'foreign key', the db will reject it. SO, can use validation.
    }


}
