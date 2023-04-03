using System.Text.Json.Serialization;
using LitovchenkoApp.Db;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestBody;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbAppContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CountriesRepository>();
builder.Services.AddScoped<ProvincesRepository>();

var app = builder.Build();
app.UseExceptionHandler("/exception");

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<DbAppContext>())
    context!.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
//HACK: for mocking Programm it should be public
public partial class Program { }