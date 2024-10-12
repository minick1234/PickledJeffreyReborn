using System.Net.Http.Headers;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MrJeffreyThePickle;
using Newtonsoft.Json;

class Program
{
    private static string? DiscordBotAPIKey;

    private static DiscordSocketClient? _client;
    private static IServiceProvider? _serviceProvider;
    private static LoggingService? _loggingService;
    private static SlashCommandHandlerService? _slashCommandHandlerService;
    private static AdminCommandHandler _adminCommandHandler;
    private static GeneralCommandHandler _generalCommandHandler;
    private static ChatGPTCommandHandlerService _chatGPTCommandHandlerService;
    private static CommandBuilderHandler _commandBuilderHandler;

    private static async Task Main(string[] args)
    {
        //Grab the environment variable for the discord key.
        await RetrieveEnvironmentVariables();

        _serviceProvider = CreateServiceProvider();
        GetRequiredServices();

        _client!.SlashCommandExecuted += HandleSlashCommandAsync;
        _client.Log += _loggingService!.LogAsync;

        await _client.LoginAsync(Discord.TokenType.Bot, DiscordBotAPIKey);
        await _client.StartAsync();

        _client.MessageDeleted += MessageDeleted;
        _client.MessageReceived += MessageRecieved;
        _client.MessageUpdated += MessageUpdated;

        _client.Ready += async () =>
        {
            await _loggingService.LogAsync(new LogMessage(LogSeverity.Info, "General",
                "The bot has connected successfully."));
            var guild = _client.GetGuild(1250886094682198176);
            await RegisterCommandHandlers();
        };

        await Task.Delay(-1);
    }

    private static async Task RegisterCommandHandlers()
    {
        try
        {
            await _commandBuilderHandler.BuildAllCommands();
        }
        catch (HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private static async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after,
        ISocketMessageChannel channel)
    {
        // Try to get the original (before) message from cache or download it from Discord
        var oldMessage = await before.GetOrDownloadAsync();
        if (oldMessage != null)
        {
            // Message was found in the cache
            Console.WriteLine(
                $"Message edited in {channel.Name}\nBefore: {oldMessage.Content}\nAfter: {after.Content}");
        }
        else
        {
            // Message not found in cache, but the new (after) message is still available
            Console.WriteLine(
                $"Message edited in {channel.Name}, but the original message was not cached.\nAfter: {after.Content}");
        }
    }

    private static async Task MessageDeleted(Cacheable<IMessage, ulong> deletedMessage,
        Cacheable<IMessageChannel, ulong> channel)
    {
        var deletedMessageContent = await deletedMessage.GetOrDownloadAsync();
        var channelContent = await channel.GetOrDownloadAsync();

        if (deletedMessageContent != null)
        {
            Console.WriteLine(
                $"Message was deleted from: {channelContent.Name}\nDeleted Message: {deletedMessageContent.Content}");
        }
        else
        {
            Console.WriteLine($"Message was deleted from {channelContent.Name} - Message Unknown");
        }
    }

    private static async Task MessageRecieved(SocketMessage message)
    {
        if (message.Author.IsBot)
        {
            Console.WriteLine("The bot has sent a message");
            return;
        }

        Console.WriteLine($"New Message recieved: {message.Content} in Channel:{message.Channel.Name}");
    }

    private static async Task HandleSlashCommandAsync(SocketSlashCommand command)
    {
        await _slashCommandHandlerService?.HandleCommandAsync(command)!;
    }

    #region Intial Setups

    private static Task RetrieveEnvironmentVariables()
    {
        DiscordBotAPIKey = Environment.GetEnvironmentVariable("DISCORD_BOT_API_KEY");
        if (!string.IsNullOrEmpty(DiscordBotAPIKey))
        {
            _loggingService?.LogAsync(new LogMessage(LogSeverity.Info, "General",
                "Discord Bot API Key and Bot API Key sucessfully retrieved."));
        }
        else
        {
            _loggingService?.LogAsync(new LogMessage(LogSeverity.Warning, "WARNING",
                "The discord bot API Key is either empty or null!"));
        }

        return Task.CompletedTask;
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var collection = new ServiceCollection();

        var config = new DiscordSocketConfig
        {
            MessageCacheSize = 1000,
            GatewayIntents = GatewayIntents.All
        };

        collection
            .AddSingleton(config)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<LoggingService>()
            .AddSingleton<CommandBuilderHandler>()
            .AddSingleton<AdminCommandHandler>()
            .AddSingleton<GeneralCommandHandler>()
            .AddSingleton<SlashCommandHandlerService>()
            .AddSingleton<ChatGPTCommandHandlerService>();
        return collection.BuildServiceProvider();
    }

    private static void GetRequiredServices()
    {
        if (_serviceProvider == null) return;
        _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();
        _loggingService = _serviceProvider.GetRequiredService<LoggingService>();
        _adminCommandHandler = _serviceProvider.GetRequiredService<AdminCommandHandler>();
        _generalCommandHandler = _serviceProvider.GetRequiredService<GeneralCommandHandler>();
        _slashCommandHandlerService = _serviceProvider.GetRequiredService<SlashCommandHandlerService>();
        _chatGPTCommandHandlerService = _serviceProvider.GetRequiredService<ChatGPTCommandHandlerService>();
        _commandBuilderHandler = _serviceProvider.GetRequiredService<CommandBuilderHandler>();
    }

    #endregion
}