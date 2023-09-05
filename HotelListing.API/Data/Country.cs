namespace HotelListing.API.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        //kw - Set up one-to-many - One country can have many hotels
        public virtual IList<Hotel> Hotels { get; set; }
    }
}