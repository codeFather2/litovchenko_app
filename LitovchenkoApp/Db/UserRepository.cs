namespace LitovchenkoApp.Db;

using LitovchenkoApp.Models;
using LitovchenkoApp.Exceptions;
using Microsoft.EntityFrameworkCore;

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
        return await db.SaveChangesAsync();
    }

    public async Task<User?> GetUser(string email)
    {
        return await db.Users.Include(u => u.Province).ThenInclude(p => p == null ? null : p.Country)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}