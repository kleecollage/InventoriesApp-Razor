using Microsoft.AspNetCore.Identity;
using RPInventories.Models;
using RPInventories.VewModels;

namespace RPInventories.Helpers;

public class FactoryUser
{
    private readonly IPasswordHasher<User> _passwordHasher;
    public FactoryUser (IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public User CreateUser(UserRegisterViewModel userVm)
    {
        var user = new User()
        {
            Id = userVm.Id,
            LastName = userVm.LastName,
            Phone = userVm.Phone,
            Email = userVm.Email,
            Name = userVm.Name,
            ProfileId = userVm.ProfileId,
            Username = userVm.Username
        };
        user.Password = _passwordHasher.HashPassword(user, userVm.Password);
        return user;
    }

    public UserEditViewModel CreateUserEdit(User user)
    {
        return new UserEditViewModel()
        {
            Id = user.Id,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
            Name = user.Name,
            ProfileId = user.ProfileId,
            Username = user.Username
        };
    }

    public void UpdateDataUser(UserEditViewModel user, User userDb)
    {
        userDb.Phone = user.Phone;
        userDb.Email = user.Email;
        userDb.Name = user.Name;
        userDb.LastName = user.LastName;
        userDb.ProfileId = user.ProfileId;
    }
}

    









