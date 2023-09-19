using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Contracts;
using AutoMapper;
using HotelListing.API.Models.Hotels;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        //private readonly HotelListingDbContext _context; //get rid of this original one and replace below

        //SUBSTITUTE the original db _context above, with the Repository!!
        private readonly IHotelsRepository _hotelsRepository; //WHY? Because we DO NOT want a direct ref to db _context in the Controller.
        private readonly IMapper _mapper;

        //"Inject" db context INTO the Controller - See program.cs, builder.Services.AddDbContext()
        //public HotelsController(HotelListingDbContext context, IHotelsRepository hotelsRepository)
        public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)

        {
            //_context = context; //orig
            this._hotelsRepository = hotelsRepository;
            this._mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        //NOTE: ActionResult<...> denotes a RETURN TYPE of IEnumerable<hotelDto> 
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            //return await _context.Hotels.ToListAsync();
            //return await this._hotelsRepository.GetAllAsync(); //now using NEW context private field - AND change ToListAsync() to GetAllAsync()

            //do the query
            var hotels = await _hotelsRepository.GetAllAsync();

            //do the mapping, and return the shit
            return this._mapper.Map<List<HotelDto>>(hotels); //RETURN the mapped Dto - MAP the hotels returned from GetAllAsync(), INTO type HotelDto

            //NOTE: Optionally wrap the return in 'Ok', like this:  return Ok(this._mapper.Map<List<HotelDto>>(hotels));
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            //var hotel = await _context.Hotels.FindAsync(id);
            var hotel = await this._hotelsRepository.GetAsync(id); //now using NEW context private field - AND change FindAsync(id) to GetAsync(id)

            if (hotel == null)
            {
                return NotFound();
            }

            //return hotel; //ORIG
            //OK() is optional
            return Ok(this._mapper.Map<HotelDto>(hotel)); //return the hotel dto - MUST RETURN SAME TYPE AS IN METHOD SIGNATURE, ABOVE
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest(id.ToString() + " does not match hotelDto.id: " + hotelDto.Id.ToString());
            }

            //_context.Entry(hotel).State = EntityState.Modified; //ORIG generated automatically - "stage" the change

            var hotel = await _hotelsRepository.GetAsync(id); //Get the hotel from the db

            if (hotel == null)
            {
                return NotFound();
            }

            _mapper.Map(hotelDto, hotel); //hotelDto is the SOURCE

            try
            {
                //await _context.SaveChangesAsync(); //ORIG "commit" the change
                await this._hotelsRepository.UpdateAsync(hotel); //replace SaveChangesAsync() with UpdateAsync(hotel)
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
        {
            //_context.Hotels.Add(hotel);
            //await _context.SaveChangesAsync();

            var hotel = _mapper.Map<Hotel>(hotelDto); //CONVERT hotelDto to a Hotel object
            await this._hotelsRepository.AddAsync(hotel); //AddAsync() INHERITED from GenericRepository!!

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            //var hotel = await _context.Hotels.FindAsync(id); //ORIG auto generated
            var hotel = await this._hotelsRepository.GetAsync(id); //replace with this - GetAsync() INHERITED from GenericRepository

            if (hotel == null)
            {
                return NotFound();
            }

            //_context.Hotels.Remove(hotel); //ORIG auto generated
            await this._hotelsRepository.DeleteAsync(id); //replace with DeleteAsync() INHERITED from GenericRepository
            //await _context.SaveChangesAsync(); //ORIG auto generated

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            //return _context.Hotels.Any(e => e.Id == id);
            return await this._hotelsRepository.Exists(id);
        }
    }
}
