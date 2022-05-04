using isdayoff;

namespace DutyBot.Services;

public class DayOffService
{
    private readonly IsDayOff _isDayOff;

    public DayOffService()
    {
        var settings = IsDayOffSettings.Build
                           .UseInMemoryCache()
                           .UseDefaultCountry(isdayoff.Contract.Country.Russia)
                           .Create();

        _isDayOff = new IsDayOff(settings);
    }

    public async Task<bool> IsWorking(DateTime dateTime)
    {
        var response = await _isDayOff.CheckDayAsync(dateTime);

        return response is not isdayoff.Contract.DayType.NotWorkingDay;
    }
}
