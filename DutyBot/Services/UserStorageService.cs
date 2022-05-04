using DutyBot.Models;
using DutyBot.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DutyBot.Services;

public class UserStorageService
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public UserStorageService()
    {
        _deserializer = new DeserializerBuilder()
            .Build();

        _serializer = new SerializerBuilder()
            .Build();
    }

    public async Task<List<User>> GetUsers()
    {
        var yml = await File.ReadAllTextAsync(FileOption.FilePath);

        var users = _deserializer.Deserialize<List<User>>(yml);

        return users;
    }

    public async Task<User> GetCurrent()
    {
        var yml = await File.ReadAllTextAsync(FileOption.CurrentUserPath);

        var user = _deserializer.Deserialize<User>(yml);

        return user;
    }

    public async Task<User> SetCurrent(User user)
    {
        var yml = _serializer.Serialize(user);

        await File.WriteAllTextAsync(FileOption.CurrentUserPath, yml);

        return user;
    }
}