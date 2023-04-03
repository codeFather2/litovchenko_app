using LitovchenkoApp.Db;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace LitovchenkoApp.Tests.Utils
{
    public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.Single(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DbAppContext>));

                services.Remove(dbContextDescriptor);

                services.AddSingleton<DbConnection, SqliteConnection>(serviceProvider =>
                {
                    var connection = new SqliteConnection("DataSource=file::memory:?cache=shared");
                    connection.Open();
                    return connection;
                });

                services.AddDbContext<DbAppContext>((serviceProvider, options) =>
                {
                    var connection = serviceProvider.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

                var provider = services.BuildServiceProvider();
                using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                using (var context = scope.ServiceProvider.GetService<DbAppContext>())
                    context!.Database.EnsureCreated();
            });

            builder.UseEnvironment("Development");
        }
    }
}
