namespace LitovchenkoApp.Controllers;

using LitovchenkoApp.Db;
using LitovchenkoApp.Exceptions;
using LitovchenkoApp.Models;
using LitovchenkoApp.Utils;
using LitovchenkoApp.Validators;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/{controller}")]
public class RegistrationController : ControllerBase
{
    private readonly UserRepository userRepo;

    public RegistrationController(UserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<IResult> Post(User user)
    {
        var validator = new UserValidator(user);
        if (!validator.IsValid())
        {
            throw new BadInputException(validator.ErrorMessage);
        }
        user.Password = PasswordUtils.HashPassword(user.Password);
        await userRepo.SaveUser(user);
        return Results.Created("registration", user.Email);
    }
}