using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutyBot.Models;

namespace DutyBot.Services;

public class UserService
{
    private readonly UserStorageService _userStorageService;

    public UserService(
        UserStorageService userStorageService)
    {
        _userStorageService = userStorageService;
    }

    public async Task<User> GetNext()
    {
        var users = await _userStorageService.GetUsers();
        var current = await _userStorageService.GetCurrent();

        return current.NextId.HasValue
            ? users.First(x => x.Id == current.NextId)
            : users.First();
    }

    public async Task<User> SetNext()
    {
        var next = await GetNext();

        return await _userStorageService.SetCurrent(next);
    }

    public async Task<User> Current()
    {
        return await _userStorageService.GetCurrent();
    }
}
