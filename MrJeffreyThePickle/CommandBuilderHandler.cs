using System;
using Discord;
using Discord.WebSocket;

namespace MrJeffreyThePickle;

public class CommandBuilderHandler
{
    private readonly DiscordSocketClient _client;
    
    //All commands list.
    private List<List<SlashCommandBuilder>> _AllCommands = new List<List<SlashCommandBuilder>>();
    
    public CommandBuilderHandler(DiscordSocketClient client)
    {
     _client = client;
    }
    
    private void BuildAdminCommands()
    {
        var commands = new List<SlashCommandBuilder>
        {
            new SlashCommandBuilder()
            .WithName("ban")
            .WithDescription("Bans a specific user")
            .AddOption("user",ApplicationCommandOptionType.User, "The user you would like to ban!", true)
            .AddOption("length", ApplicationCommandOptionType.Integer, "The amount of time the ban should last.", false)
            .AddOption("reason",ApplicationCommandOptionType.String, "The reason for the ban!", false)
            .AddOption("delete_messages", ApplicationCommandOptionType.Boolean, "If the banned user's messages should be cleared", false)
        };
        
        _AllCommands.Add(commands);
    }
    
    private void BuildGeneralCommands()
    {
        var commands = new List<SlashCommandBuilder>
        {
            new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("PONG"),

            new SlashCommandBuilder()
                .WithName("tts_toggle")
                .WithDescription("Toggle TTS for bot responses."),
            
            new SlashCommandBuilder()
                .WithName("echo")
                .WithDescription("Make PickledJeffrey repeat your message")
                .AddOption("message",ApplicationCommandOptionType.String, "The message you would like to be repeated", true),
            
            new SlashCommandBuilder()
                .WithName("pickledjeffrey_dm_user")
                .WithDescription("Make Pickled Jeffrey DM a user with a message")
                .AddOption("user", ApplicationCommandOptionType.User, "The user you would like to message", true)
                .AddOption("message", ApplicationCommandOptionType.String, "The message you would like to send", true)
            
        };
        
        _AllCommands.Add(commands);
    }
    
    private void BuildChatGPTCommands()
    {
        var commands = new List<SlashCommandBuilder>()
        {
            new SlashCommandBuilder()
                .WithName("picklegpt")
                .WithDescription("Ask PickleGPT any question! Fully integrated with ChatGPT")
                .AddOption("question", ApplicationCommandOptionType.String,
                    "The message you would like to send to pickleGPT", true)
        };

        _AllCommands.Add(commands);
    }
    
    public async Task BuildAllCommands()
    {
        BuildAdminCommands();
        BuildGeneralCommands();
        BuildChatGPTCommands();

        await RegisterSlashCommands();
    }
    
    public async Task BuildAllCommands(SocketGuild guild)
    {
        BuildAdminCommands();
        BuildGeneralCommands();
        BuildChatGPTCommands();

        await RegisterSlashCommands(guild);
    }

    private async Task RegisterSlashCommands()
    {
        foreach (var Commands in _AllCommands)
        {
            foreach (var slashCommand in Commands)
            {
                await _client.CreateGlobalApplicationCommandAsync(slashCommand.Build());
            }
        }
    }
    
    private async Task RegisterSlashCommands(SocketGuild guild)
    {
        foreach (var Commands in _AllCommands)
        {
            foreach (var slashCommand in Commands)
            {
                await guild.CreateApplicationCommandAsync(slashCommand.Build());
            }
        }
    }
}