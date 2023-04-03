namespace LitovchenkoApp.Db;

using LitovchenkoApp.Models;
using LitovchenkoApp.Exceptions;
using LitovchenkoApp.Logging;

public class ProvincesRepository
{
    private DbAppContext db;
    private ILogger<ProvincesRepository> logger;

    public ProvincesRepository(DbAppContext db, ILogger<ProvincesRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<int> SaveProvince(Province province)
    {
        if (db.Provinces.Any(p => p.Name == p.Name && p.CountryId == province.CountryId))
        {
            throw new RecordAlreadyExistsException($"Country {province.Name} already exists");
        }
        db.Provinces.Add(province);
        var result = await db.SaveChangesAsync();
        if (result >= 0)
        {
            logger.LogInformation(LoggingEvents.DbCrud, "Province {province} created with id {id}", province.Name, province.Id);
        }
        return result;
    }

    public IEnumerable<Province> GetProvincesForCountry(int countryId)
    => db.Provinces.Where(p => p.CountryId == countryId);
}