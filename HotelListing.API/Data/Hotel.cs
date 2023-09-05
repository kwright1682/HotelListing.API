using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Data
{

    //kw - BEFORE we can add a table, we must FIST define a CLASS that has fields/columns
    //  of what we EXPECT to be in the corresponding table.  INCL's foreign keys...etc.

    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        //kw - We are using a "Code first" approach where VS will generate stuff FOR US! The other approach would be "database first".
        //kw - Set up a foreign key reference between a Hotel and the Country
        [ForeignKey(nameof(CountryId))] //kw - Could also use literal string "CountryId" - Using nameof() strongly typed, and prevents typo
        public int CountryId { get; set; } //kw - will be a foreign key
        public Country Country { get; set; } //kw - Created class Country via right-click 'Data'|Add|Class, which gets added to 'Data' folder

    }
}
