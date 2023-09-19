using HotelListing.API.Contracts;
using HotelListing.API.Data;

namespace HotelListing.API.Repository
{
    //DON'T FORGET to REGISTER each repository in Program.cs

    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        //"daisy-chain" thiscontext down to the base class, GenericRepository.
        //  This is HOW the base-class gets a db-context.
        public HotelsRepository(HotelListingDbContext thisContext) : base(thisContext)
        {
            //For now, there's nothing special to do here.
            //  BUT, if/when we need something in this constructor that is custom to HotelsRepository, then
            //  you have to first DEFINE it in IHotelsRepository, and then IMPLEMENT it here.
        }
    }
}
