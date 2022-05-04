namespace DutyBot.Models;

public class User
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Ссылка на телеграм пользователя
    /// </summary>
    public string Telegram { get; set; }
    /// <summary>
    /// Id следующего пользователя
    /// </summary>
    public int? NextId { get; set; }

    public override string ToString()
    {
        return $"{Name}: {Telegram}";
    }
}
