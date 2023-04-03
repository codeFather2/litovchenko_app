namespace LitovchenkoApp.Db;

using LitovchenkoApp.Models;
using LitovchenkoApp.Exceptions;
using LitovchenkoApp.Logging;

public class CountriesRepository
{
    private DbAppContext db;
    private ILogger<CountriesRepository> logger;

    public CountriesRepository(DbAppContext db, ILogger<CountriesRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<int> SaveCountry(Country country)
    {
        if (db.Countries.Any(u => u.Name == u.Name))
        {
            throw new RecordAlreadyExistsException($"Country {country.Name} already exists");
        }
        db.Countries.Add(country);
        var result = await db.SaveChangesAsync();
        if (result >= 0)
        {
            logger.LogInformation(LoggingEvents.DbCrud, "Country {country} created with id {id}", country.Name, country.Id);
        }
        return result;
    }

    public IEnumerable<Country> GetCountries()
    => db.Countries;
}