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
    public IResult Get(int countryId)
    => Results.Ok(provincesRepo.GetProvinces(countryId));

}