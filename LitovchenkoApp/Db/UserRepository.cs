namespace LitovchenkoApp.Db;

using LitovchenkoApp.Models;
using LitovchenkoApp.Exceptions;
using Microsoft.EntityFrameworkCore;
using LitovchenkoApp.Logging;

public class UserRepository
{
    private DbAppContext db;
    private ILogger<UserRepository> logger;

    public UserRepository(DbAppContext db, ILogger<UserRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<int> SaveUser(User user)
    {
        if (db.Users.Any(u => u.Email == user.Email))
        {
            throw new RecordAlreadyExistsException($"User with email {user.Email} already exists");
        }
        db.Users.Add(user);
        var result = await db.SaveChangesAsync();
        if (result >= 0)
        {
            logger.LogInformation(LoggingEvents.DbCrud, "User {user} created with id {id}", user.Email, user.Id);
        }
        return result;
    }

    public async Task<User?> GetUser(string email)
    {
        return await db.Users.Include(u => u.Province).ThenInclude(p => p == null ? null : p.Country)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}