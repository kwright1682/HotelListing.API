namespace HotelListing.API.Data
{
    public class Country
    {
        public int Id { get; set; } //For Dto - DO NOT Want User to provide

        //So, this means I need to tell the User to send me a JSON object type that
        //  has/represents only Name and ShortName. 
        public string Name { get; set; } //For Dto - DO Want User to provide
        public string ShortName { get; set; } //For Dto - DO Want User to provide

        //ALSO - We probably don't want User to send a List of Hotels when
        //  just creating a country (via POST).  BUT, it is UP TO YOU what you want
        //  User to provide to your API.
        public virtual IList<Hotel> Hotels { get; set; }  //kw - Set up one-to-many - One country can have many hotels
    }
}