using HotelListing.API.Configurations;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//////////////// All the services to be configured ///////////////////////////
///The 'Builder' constructs all the services (and anything that needs to be injected...ie dependencies).
///By the time the app is RUN (below), all those things need to be in place so that
///they can be accessed (IOC Container - 'inversion-of-control' container).

// Add services to the container.

//kw - Note that 'HotelListingDbConnectionString' is defined in appsetting.json
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");

//"register" the db context AS PART OF OUR 'SERVICES'
//By doing this, you can then "inject" the context into pretty much anywhere it's needed
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//////////////// END services to be configured ///////////////////////////
///

/////////////// CORS //////////////////
//kw - Prepare this API for 3rd Party access, ie apps call this API from machines that are NOT on same server as this API.
//kw - Create the 'POLICY' and add it to the 'Services'
//  "AllowAll" is the POLICY NAME.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
////////////// END CORS ///////////////


//kw:
//Where ctx: context,  lc: logger configuration
//See Serilog configuration in appsettings.json
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//Add automapper - Adding this to Services makes automapper INJECTABLE (anywhere).
//'MapperConfig' is the class you created in MapperConfig.cs, and added to NEW FOLDER 'Configurations', under the HotelListing.API project.
builder.Services.AddAutoMapper(typeof(MapperConfig)); //Don't forget to add 'using'

//add Repositories -    AddScoped(interface, implementation-type)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>(); // Addscoped<Interface, implementation>


var app = builder.Build(); //So, by the time it hits THIS line, the app has everything it needs to Build.

//////////////////// All the "middleware" stuff goes here ///////////////
// Configure the HTTP "request pipeline" ("middleware").
//kw NOTE: You can create CUSTOMIZED "middleware" if you want.
if (app.Environment.IsDevelopment()) //kw - Remove this condition if you want Swagger in PRODUCTION. Else leave 'IsDevelopment'.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection(); //ensure using https

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
//////////////////// END "middleware" stuff ///////////////

app.Run();
