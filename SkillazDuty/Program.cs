using System;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using DutyBot.Commands;
using DutyBot.Cron;
using DutyBot;
using DutyBot.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Bot>();
builder.Services.AddScoped<SenderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserStorageService>();

builder.Services.AddScoped<Command, NextCommand>();
builder.Services.AddScoped<Command, HealthCheckCommand>();
builder.Services.AddScoped<Command, ChatIdCommand>();
builder.Services.AddScoped<Command, WhyCommand>();

builder.Services.AddTransient<JobFactory>();
builder.Services.AddScoped<DutyJob>();

builder.Services.AddSingleton<DayOffService>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse(builder.Configuration["Host"]));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllers();

app.MapPost("api/message",
    async (HttpContext ctx,
     [FromBody] object raw,
     [FromServices] IEnumerable<Command> commands,
     [FromServices] ILogger<object> logger) =>
{
    logger.LogInformation(raw.ToString());
    var update = JsonConvert.DeserializeObject<Update>(raw.ToString());

    if (string.IsNullOrEmpty(update.Message?.Text))
    {
        logger.LogInformation("message text is empty");
        return;
    }

    if (update.Message?.Chat?.Id.ToString() != builder.Configuration["ChatId"])
    {
        logger.LogInformation("incorrect chat");
        return;
    }

    var command = commands.FirstOrDefault(x => x.Contains(update.Message.Text));
    if (command is not null)
    {
        logger.LogInformation("Executing command with text {text}", update.Message.Text);
        try
        {
            await command.Execute(update.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ctx.Request.GetHashCode(), ex, "error executing command {text}", update.Message.Text);
        }
    }
});

app.MapPost("api/next",
    async (HttpContext ctx,
     [FromServices] IEnumerable<Command> commands,
     [FromServices] IConfiguration configuration,
     [FromServices] ILogger<object> logger) =>
{
    var name = configuration["Name"];
    var command = commands.FirstOrDefault(x => x.Contains($"/next@{name}"));
    if (command is not null)
    {
        try
        {
            await command.Execute(null);
        }
        catch (Exception ex)
        {
            logger.LogError(ctx.Request.GetHashCode(), ex, "error next");
        }
    }
});

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        Scheduler.Start(serviceProvider);
        await Bot.CreateClient(builder.Configuration);
    }
    catch (Exception e)
    {
        serviceProvider.GetRequiredService<ILogger<object>>().LogError(e, "Exception start service");
        throw;
    }

    
}

app.Run();
