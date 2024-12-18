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
        return new UserEditViewModel
        {
            Id = user.Id,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
            Name = user.Name,
            ProfileId = user.ProfileId,
            Username = user.Username,
            Photo = user.Photo
        };
    }

    public void UpdateDataUser(UserEditViewModel userVm, User userDb)
    {
        userDb.Phone = userVm.Phone;
        userDb.Email = userVm.Email;
        userDb.Name = userVm.Name;
        userDb.LastName = userVm.LastName;
        userDb.ProfileId = userVm.ProfileId;
    }

    public UserChangePasswordViewModel CreateUserChangePassword(User user)
    {
        return new UserChangePasswordViewModel
        {
            Id = user.Id,
            Username = user.Username,
        };
    }

    public void UpdatePasswordUser(UserChangePasswordViewModel userVm, User userDb)
    {
        userDb.Password = _passwordHasher.HashPassword(userDb, userVm.Password);
    }
}

    









