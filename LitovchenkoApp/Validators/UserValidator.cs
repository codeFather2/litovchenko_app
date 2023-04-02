using LitovchenkoApp.Models;
using LitovchenkoApp.Utils;

namespace LitovchenkoApp.Validators;

public class UserValidator
{
    public UserValidator(User user)
    {
        User = user;
    }

    public string ErrorMessage { get; private set; } = string.Empty;

    public User User { get; }

    public bool IsValid()
    {
        if (!PasswordUtils.IsPasswordSafe(User.Password))
        {
            ErrorMessage = "Weak user password";
            return false;
        }
        if (User.ProvinceId <= 0)
        {
            ErrorMessage = "User should have province";
            return false;
        }
        return true;
    }

}