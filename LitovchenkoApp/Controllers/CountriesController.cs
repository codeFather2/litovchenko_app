namespace LitovchenkoApp.Controllers;
using LitovchenkoApp.Db;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/{controller}")]
public class CountriesController : ControllerBase
{
    private CountriesRepository countriesRepo;

    public CountriesController(CountriesRepository countriesRepo)
    {
        this.countriesRepo = countriesRepo;
    }

    [HttpGet]
    public IResult Get()
    => Results.Ok(countriesRepo.GetCountries());

}