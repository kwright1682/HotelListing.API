using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.ModelsDto.Country;
using AutoMapper;
using HotelListing.API.Models.Country;
using HotelListing.API.Contracts;

/*
kw -
EXCELLENT - https://blog.ploeh.dk/2017/01/27/dependency-injection-is-passing-an-argument/

POST - Typical API Example/pattern:
public IHttpActionResult Post(ReservationRequestDto dto)
{
    var validationMsg = validator.Validate(dto);
    if (validationMsg != "")
        return this.BadRequest(validationMsg);
 
    var r = mapper.Map(dto);
    var id = maîtreD.TryAccept(r);
    if (id == null)
        return this.StatusCode(HttpStatusCode.Forbidden);
 
    return this.Ok();
}

1. validate input
2. map to a domain model
3. delegate to said model
4. examine posterior state
5. return a result.
*/

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //attribute
    public class CountriesController : ControllerBase //inherit from ControllerBase
    {
        //private readonly HotelListingDbContext _context; //<-----------COPIED this line TO GenericRepository class CONSTRUCTOR
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;

        //"Inject" db context INTO the Controller - See program.cs, builder.Services.AddDbContext()
        //public CountriesController(HotelListingDbContext context, IMapper mapper) //ORIG
        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
        {
            //_context = context; //assign 'private' field, above. This way, we don't have to instantiate a new db context ea. time it's needed...just use the private '_context'
            this._countriesRepository = countriesRepository;    //refactored
            this._mapper = mapper; //THIS IS THE 'INJECTION' of the mapper. We will use it in the POST.
        }

        //ENDPOINT
        // GET: api/Countries
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Country>>> GetCountries() //ORIG
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries() //NEW way used the Dto
        {
            //NOTE: IEnumerable is just a 'collection' type...like an array[] is just a 'collection' type

            //This line basically means, Select * from Countries
            //return await _context.Countries.ToListAsync(); //If no error, then assume an Ok() 200 response (see note below)

            //NOTE: If for some reason you wanted to return an http status code 200 EXPLICITLY (on success), then
            //  you could wrap the call with 'Ok()'...like this:
            //          return Ok(await _context.Countries.ToListAsync());

            //For clarity, you can split it up like this, if you want - Improves readability
            var countries = await this._countriesRepository.GetAllAsync(); //get a List of Countries from the db
            var countriesList = _mapper.Map<List<GetCountryDto>>(countries); //now Map the data according to the Dto fields - FROM a List TO a List
            //return Ok(countries); //ORIG
            return Ok(countriesList); //Each country in 'countriesList' will contain ONLY the fields defined in the GetCountryDto (country id, any fields in it's base-class, PLUS a list of Hotels)

            //BTW - AutoMapper DOES NOT alert you for data-type compatibility!!
            //      In this particular case, it did not alert me that I needed a typecast to <List... ((ie, can't map a list (array) into a single object)), so
            //      we added the <List... typecast MANUALLY.
            //      So, you have to look out for this.
        }


        //ENDPOINT
        // GET: api/Countries/5
        [HttpGet("{id}")] // Think of {id} as a TEMPLATE - We expect an 'id' param on this GET request
        //NOTE: IF you were to REMOVE the {id} template, then you would have TWO
        //  identical GET endpoints, and EntityFramework would not know which one to call as
        //  it would be AMBIGUOUS.  YOU CANNOT HAVE TWO OR MORE IDENTICAL ENDPOINTS...IN THIS CASE, for 'GET'
        //........The fact that one method name is 'GetCountries()' and the other is 'GetCountry()'
        //          makes no difference. There would be TWO 'GET' endpoints that are identical
        //          in the fact that they are BOTH 'GET's, and take no params.
        //SO ... You CAN have MULTIPLE 'GET's, but their [httpGet] SIGNATURES must DIFFER.
        //---------------------
        //"I know as a developer that I can have two methods with different names. WHY ARE THESE CONFLICTING NOW?"
        //Ans: It is because, by removing the {id} template, the address 'api/Countries/5' becomes 'api/Countries/' which
        //  is the SAME as the first one above.  Both of these CANNOT be 'api/Countries/' in the same Controller, for
        //  the same type of GET request. You CANNOT have a REST API with two identical Endpoints.
        //RULE: You CAN have multiple GET/POST/PUT/DELETE etc in the same Controller.  But, If you DO, then EACH {template} MUST BE UNIQUE.
        
        //public async Task<ActionResult<Country>> GetCountry(int id)
        public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
        {
            //NOTICE here that we are NOT returning a collection type like IEnumerable
            //  We're just returning one Country object, the one whose 'id' matches.
            //var thisCountry = await _context.Countries.FindAsync(id);
            //var country = _mapper.Map<GetCountryDto>(thisCountry); //now Map the data INTO the Dto

            //NEW Re-factored query (refactored after adding 'HotelDto')
            //----------------------------------------------------------
            //This line says:  
            //1. _context: go to the database
            //2. go to the Countries table
            //3. .Include() the list of Hotels
            //4. ...and Fetch the 1st record that has the 'id' that matches the input param 'id'
            //var thisCountry = await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(thisId => thisId.Id == id); //ORIG
            //var thisCountry = await this._countriesRepository.GetAsync(id); //.Include(c => c.Hotels).FirstOrDefaultAsync(thisId => thisId.Id == id);
            
            //The Controller does not know or care HOW the Details are being retrieved.
            var thisCountry = await this._countriesRepository.GetDetails(id); //Don't forget to 'await' this call

            if (thisCountry == null)
            {
                return NotFound(); //404
            }
            //else
            var countryDto = _mapper.Map<GetCountryDto>(thisCountry); //Hey Mapper!! Please convert 'thisCountry' into 'countryDto'.

            //'Ok()' is OPTIONAL. But it IS explicit, and easier to understand.
            return Ok(countryDto); //If everything is ok (thisCountry <> null), then return it
        }

        //ENDPOINT
        // PUT: api/Countries/5  (PUT is an UPDATE)
        // To protect from OVERPOSTING attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            //PUT REPLACES existing data with NEW data (without comparison to what was there before)
            
            //Check that 'id' param is SAME as country.id
            //  If they're different, then Bad Request
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid record id"); //...whatever "message" you want
            }

            //_context.Entry(updateCountryDto).State = EntityState.Modified; //other EntityState's are Unchanged, Added, Deleted...etc
            //var country = await _context.Countries.FindAsync(updateCountryDto.Id); //retrieve the Country
            var thisCountry = await this._countriesRepository.GetAsync(id);

            if (thisCountry == null)
            {
                return NotFound();
            }

            //Country IS found
            //Now, take the data (ie, all the fields that map over) from passed in updateCountryDto, and use it to change the data in 'country'
            _mapper.Map(updateCountryDto, thisCountry); //take everything from LEFT object, and map it into the RIGHT object

            try
            {
                //SaveChanges "knows" it has been "Modified" due to setting EntityState, internally.
                //await _context.SaveChangesAsync();
                await this._countriesRepository.UpdateAsync(thisCountry); //do the update
            }
            catch (DbUpdateConcurrencyException) //multiple user's concurrently try to Update/Delete/Modify, etc
            {
                if (! await CountryExists(id)) //CountryExists() boolean method, below
                {
                    return NotFound();
                }
                else
                {
                    throw; //some concurrent operation was happening
                }
            }

            //NoContent(): poorly named, means "I did the operation, but don't have anything to show" - Status 204
            //Returns 'NoContent' object as the response.
            return NoContent(); 
        }

        //ENDPOINT
        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]

        //'PostCountry()' returns an object of type 'Country'
        //So, we have a Task that returns an ActionResult of type Country.

        //public async Task<ActionResult<Country>> PostCountry(Country country) //ORIG BEFORE Replace param with a Dto
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto thisCountry) //NEW - After replace param with Dto
        {
            //REPLACE this code block with AutoMapper.
            //var newCountry = new Country
            //{
            //    Name = thisCountry.Name,
            //    ShortName = thisCountry.ShortName,
            //};
            //  INSTEAD of the code block above, let the Controller INJECT automapper, and then
            //  let automapper do the conversion FOR us.

            //FIRST: Do the INJECTION in the CountriesController CONSTRUCTOR
            //  by adding 'IMapper mapper' to the constructor params

            //THIS ONE LINE Replaces the 5 lines above
            var newCountry = _mapper.Map<Country>(thisCountry); //MAP thisCountry INTO Country



            //Take a COPY of the db context, initialize it, and then do the operation
            //This says, "hey database, get me the table called 'Countries', and ADD this NEW Country". 
            //_context.Countries.Add(newCountry); //kw - 'Add' is the 'operation' - QUEUE up the operation
            this._countriesRepository.AddAsync(newCountry); //_countriesRepository replaces the orig _context, after refactoring.




            //await _context.SaveChangesAsync(); //Now EXECUTE the operation (commit) --- Now being done in GenericRepository

            //Return the details of the newly created Country
            //Retrieve the newly added country via url "GetCountry" (above), for the client (ie, the caller)
            return CreatedAtAction("GetCountry", new { id = newCountry.Id }, newCountry); //CreatedatAction is code 201...So, a good response
        }

        //ENDPOINT
        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            //DELETE follows the same basic pattern as POST and PUT with {id} template

            //Find the country
            //var country = await _context.Countries.FindAsync(id);

            var country = await this._countriesRepository.GetAsync(id);
            if (country == null) //does it exist?
            {
                return NotFound();
            }

            //_context.Countries.Remove(country); //EntityFramework sets state to 'deleted'
            //await _context.SaveChangesAsync(); //save db changes
            await this._countriesRepository.DeleteAsync(id);

            return NoContent(); //Same as PUT
        }

        private async Task<bool> CountryExists(int id) //Country id as param
        {
            //The inner function returns true if e.id == id, else returns false
            //Since Any() scans thru the Countries table, I **think** 'e' represents the
            //  current row of the scan.  So, if the id of the current row of the scan
            //  matches the passed-in id param, then return true, else not found and return false.

            //Any(): Determines whether any element of a sequence exists or satisfies a condition.
            //  Returns true if the source sequence contains any matching elements; otherwise, false.
            //return _context.Countries.Any(e => e.Id == id); 
            return await this._countriesRepository.Exists(id);
        }
    }
}
