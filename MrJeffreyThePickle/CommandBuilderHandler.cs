using System;
using System.Net.Http.Headers;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

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
                .AddOption("user", ApplicationCommandOptionType.User, "The user you would like to ban!", true)
                .AddOption("length", ApplicationCommandOptionType.Integer, "The amount of time the ban should last.",
                    false)
                .AddOption("reason", ApplicationCommandOptionType.String, "The reason for the ban!", false)
                .AddOption("delete_messages", ApplicationCommandOptionType.Boolean,
                    "If the banned user's messages should be cleared", false)
        };

        foreach (var command in commands)
        {
            command.WithDefaultMemberPermissions(GuildPermission.Administrator);
        }

        _AllCommands.Add(commands);
    }

    private void BuildGeneralCommands()
    {
        var commands = new List<SlashCommandBuilder>
        {
            new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("PONG").AddOption("blah", ApplicationCommandOptionType.String, "Changed the lalala", false),

            new SlashCommandBuilder()
                .WithName("tts_toggle")
                .WithDescription("Toggle TTS for bot responses."),

            new SlashCommandBuilder()
                .WithName("echo")
                .WithDescription("Make PickledJeffrey repeat your message")
                .AddOption("message", ApplicationCommandOptionType.String, "The message you would like to be repeated",
                    true),

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
                    "The message you would like to send to pickleGPT", true),

            new SlashCommandBuilder().WithName("dm_user_picklegpt_message")
                .WithDescription("Send a user a message from a highly seductive PickleGPT")
                .AddOption("user", ApplicationCommandOptionType.User,
                    "The user you would like to send a steamy PickleGPT message too", true)
                .AddOption("message", ApplicationCommandOptionType.String, "Context of your steamy response", true)
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
        //Need to implement the ability to grab the current the current guild this is registering for,
        //and make sure that if it already exists in the guild with the same options and command, we skip it
        //but if its not the same or doesnt exist we add it.

        // Fetch existing registered commands
        var registeredCommands = await _client.GetGlobalApplicationCommandsAsync();

        foreach (var Commands in _AllCommands)
        {
            foreach (var slashCommand in Commands)
            {
                var existingCommand = registeredCommands.FirstOrDefault(x => x.Name == slashCommand.Name);

                if (existingCommand != null)
                {
                    if (IsCommandDifferent(existingCommand, slashCommand))
                    {
                        // If the command has changed, delete the old command
                        await existingCommand.DeleteAsync();

                        // Then register the new command
                        await _client.CreateGlobalApplicationCommandAsync(slashCommand.Build());
                        Console.WriteLine($"Updated command: {slashCommand.Name}");
                        
                        /*
                        //For production - if commands are updated, send a message to let people know, so they can restart discord client.
                        foreach (var guild in _client.Guilds) // Loop through all guilds the bot is in
                        {
                            // Attempt to get the system channel or a general text channel
                            var channel = guild.SystemChannel ?? guild.TextChannels.FirstOrDefault();

                            if (channel != null) // Check if a suitable channel was found
                            {
                                // Send the message to the channel
                                await channel.SendMessageAsync("Commands have been updated! Please restart your Discord client to see the changes.");
                            }
                        }
                        */
                    }
                    else
                    {
                        // If the command hasn't changed, skip re-registration
                        Console.WriteLine($"Command {slashCommand.Name} has not changed, skipping rebuild...");
                    }
                }
                else
                {
                    await _client.CreateGlobalApplicationCommandAsync(slashCommand.Build());
                    Console.WriteLine($"Registered new command: {slashCommand.Name}");
                }
            }
        }
    }

    private async Task RegisterSlashCommands(SocketGuild guild)
    {
        //Need to implement a check to see if the command already exists in this guild from the global
        //context, then we need to 1.Check if the options are different from the global context and register it
        //or B.Skip it, if its the exact same.


        // Fetch existing registered commands
        var registeredCommands = await guild.GetApplicationCommandsAsync();

        foreach (var Commands in _AllCommands)
        {
            foreach (var slashCommand in Commands)
            {
                var existingCommand = registeredCommands.FirstOrDefault(x => x.Name == slashCommand.Name);

                if (existingCommand != null)
                {
                    if (IsCommandDifferent(existingCommand, slashCommand))
                    {
                        // If the command has changed, delete the old command
                        await existingCommand.DeleteAsync();

                        // Then register the new command
                        await guild.CreateApplicationCommandAsync(slashCommand.Build());
                        Console.WriteLine($"Updated command: {slashCommand.Name}");
                    }
                    else
                    {
                        // If the command hasn't changed, skip re-registration
                        Console.WriteLine($"Command {slashCommand.Name} has not changed, skipping...");
                    }
                }
                else
                {
                    await guild.CreateApplicationCommandAsync(slashCommand.Build());
                    Console.WriteLine($"Registered new command: {slashCommand.Name}");
                }
            }
        }
    }

    private bool IsCommandDifferent(SocketApplicationCommand existingCommand, SlashCommandBuilder newCommand)
    {
        // Compare the basic properties (name, description)
        if (existingCommand.Name != newCommand.Name ||
            existingCommand.Description != newCommand.Description)
        {
            return true; // The name or description changed
        }

        // If one command has options and the other doesn't, they are different
        var existingOptions = existingCommand.Options ?? new List<SocketApplicationCommandOption>();
        var newOptions = newCommand.Options ?? new List<SlashCommandOptionBuilder>();
        
        if (existingOptions.Count != newOptions.Count)
        {
            return true; // Option count changed
        }
        
        for (int i = 0; i < existingOptions.Count; i++)
        {
            var existingOption = existingOptions.ElementAt(i);

            bool optionExists = false;
            
            foreach (var newOption in newOptions)
            {
                if (newOption.Name == existingOption.Name)
                {
                    optionExists = true;               
                    // Compare option names, types, descriptions, and required status
                    if (existingOption.Type != newOption.Type ||
                        existingOption.Description != newOption.Description || 
                        (existingOption.IsRequired == true && newOption.IsRequired == false) || 
                        (existingOption.IsRequired == null && newOption.IsRequired == true))
                    {
                        return true; // Option properties changed
                    }
                }
            }
            //The name of this doesnt exist, hence there is a change between existing commands, and new commands.
            if (!optionExists)
            {
                return true;
            }
        }
        return false; // No differences found
    }

    #region DELETE ALL COMMANDS, GUILD AND GLOBAL FOR CLEANUP

    private async Task DeleteAllGlobalCommands()
    {
        // Fetch all global commands
        var globalCommands = await _client.GetGlobalApplicationCommandsAsync();

        // Loop through and delete each global command
        foreach (var command in globalCommands)
        {
            await command.DeleteAsync();
            Console.WriteLine($"Deleted global command: {command.Name}");
        }
    }

    //For now - will change later
    //But since im testing and making and deleting commands and everything so often
    //everytime i start the bot i will delete all the old commands and reregister the new ones.
    private async Task DeleteAllCommandsAsync(string DiscordBotAPIKey)
    {
        var globalCommands = await _client.GetGlobalApplicationCommandsAsync();
        foreach (var command in globalCommands)
        {
            await command.DeleteAsync();
            Console.WriteLine($"Deleted global command: {command.Name}");
        }

        // Get all commands
        var commands = await GetGuildCommandsAsync(1250886094682198176, DiscordBotAPIKey);

        // Delete each command
        foreach (var command in commands)
        {
            string commandId = command.id.ToString();
            await DeleteGuildCommandAsync(1250886094682198176, commandId, DiscordBotAPIKey);
        }
    }

    private async Task<List<dynamic>> GetGuildCommandsAsync(ulong guildId, string botToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", botToken);

        var response =
            await client.GetAsync(
                $"https://discord.com/api/v9/applications/{_client.CurrentUser.Id}/guilds/{guildId}/commands");
        response.EnsureSuccessStatusCode();

        var commandsJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<dynamic>>(commandsJson);
    }

    private async Task DeleteGuildCommandAsync(ulong guildId, string commandId, string botToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", botToken);

        var response =
            await client.DeleteAsync(
                $"https://discord.com/api/v9/applications/{_client.CurrentUser.Id}/guilds/{guildId}/commands/{commandId}");
        response.EnsureSuccessStatusCode();

        Console.WriteLine($"Deleted guild command with ID: {commandId}");
    }

    #endregion
}