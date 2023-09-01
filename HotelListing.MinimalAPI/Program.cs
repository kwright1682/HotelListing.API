var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//kw - in the Controller version, WeatherForecast was the Name of the Controller.
//  In a Minimal api, this IS the GET
app.MapGet("/weatherforecast", () =>
{
    //kw - This code is similar to what WOULD HAVE been in a Controller in a Non-Minimal api.
    //  Here, we declare the forecast 'range', and return it - which is the same thing as in
    //  the non-minimal Controller.
    //  Everything is INSIDE of HERE instead of inside a Controller.
    //  THIS IS NOT TYPICAL - Microsoft made this technique availble presumably to aid smaller
    //  projects by not burdening them with having to use the standard Controller way of doing it.
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

//kw - Here, we have a 'record' in place of a Class
internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}