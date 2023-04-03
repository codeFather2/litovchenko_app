namespace LitovchenkoApp.Controllers;
using LitovchenkoApp.Db;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/{controller}")]
public class ProvincesController : ControllerBase
{
    private ProvincesRepository provincesRepo;

    public ProvincesController(ProvincesRepository provincesRepo)
    {
        this.provincesRepo = provincesRepo;
    }

    [HttpGet]
    public IResult GetProvincesForCountry(int countryId)
    => Results.Ok(provincesRepo.GetProvincesForCountry(countryId));

}