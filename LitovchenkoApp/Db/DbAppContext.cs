namespace LitovchenkoApp.Db;

using LitovchenkoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.IO;
using LitovchenkoApp.Logging;

public class DbAppContext : DbContext
{
    private readonly string dataSeedFile;
    private readonly string dbPath;
    private readonly ILogger<DbAppContext> logger;

    public DbAppContext(IConfiguration config, ILogger<DbAppContext> logger, DbContextOptions options)
        :base(options)
    {
        var folder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()!.Location);
        dbPath = Path.Join(folder, config!.GetValue<string>("DbName"));
        dataSeedFile = config!.GetValue<string>("DataSeed") ?? "";
        this.logger = logger;
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Country> Countries { get; set; } = null!;

    public DbSet<Province> Provinces { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        if (File.Exists(dataSeedFile))
        {
            var data = JsonSerializer.Deserialize<List<Country>>(File.ReadAllText(dataSeedFile));
            if (data != default)
            {
                builder.Entity<Country>().HasData(data.Select(c => new Country { Id = c.Id, Name = c.Name }));
                builder.Entity<Province>(
                entity =>
                {
                    entity.HasOne(p => p.Country)
                        .WithMany(c => c.Provinces)
                        .HasForeignKey("CountryId");
                });
                builder.Entity<Province>().HasData(data.SelectMany(c => c.Provinces));
            }
        }
        else
        {
            logger.LogError(LoggingEvents.Error, "Cannot find seed file {file}", dataSeedFile);
        }

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured)
        {
            return;
        }
        options.UseSqlite($"Data Source={dbPath}");
    }
}