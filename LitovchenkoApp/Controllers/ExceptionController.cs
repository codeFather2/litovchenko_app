namespace LitovchenkoApp.Controllers;

using LitovchenkoApp.Exceptions;
using LitovchenkoApp.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ExceptionController: ControllerBase
{
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly ILogger<ExceptionController> logger;

    public ExceptionController(IWebHostEnvironment webHostEnvironment, ILogger<ExceptionController> logger){
        this.webHostEnvironment = webHostEnvironment;
        this.logger = logger;
    }

    [Route("exception")]
    public ExceptionResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context!.Error;
        logger.LogError(LoggingEvents.Error, exception, $"Error at {DateTime.UtcNow}");

        var code = exception switch
        {
            RecordAlreadyExistsException => HttpStatusCode.Conflict,
            BadInputException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
        if  (exception is RecordAlreadyExistsException)
            code = HttpStatusCode.Conflict;

        Response.StatusCode = (int)code;

        return new ExceptionResponse(exception, webHostEnvironment.IsDevelopment());
    }

}