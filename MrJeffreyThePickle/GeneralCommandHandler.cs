using Discord;
using Discord.WebSocket;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrJeffreyThePickle
{
    internal class GeneralCommandHandler : IRegisterSlashCommands
    {
        private readonly DiscordSocketClient _client;

        public GeneralCommandHandler(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("tts_toggle")]
        private async Task HandleTTSToggleAsync(SocketSlashCommand command)
        {
            TTSStateHandlerService.IsResponsesTts = !TTSStateHandlerService.IsResponsesTts;
            if (TTSStateHandlerService.IsResponsesTts)
            {
                await command.RespondAsync("TTS is enabled", null, TTSStateHandlerService.IsResponsesTts);
            }
            else
            {
                await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
            }
        }

        [Command("ping")]
        private async Task HandlePingCommandAsync(SocketSlashCommand command)
        {
            var message = command.Data.Options.FirstOrDefault(n => n.Name == "blah")?.Value;
            Console.WriteLine(message);
            
            await command.RespondAsync("PONG MY BOY", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("echo")]
        private async Task HandleEchoCommandAsync(SocketSlashCommand command)
        {
            var message = (string)command.Data.Options.First(m => m.Name == "message").Value;

            await command.RespondAsync(message, null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("pickledjeffrey_dm_user")]
        private async Task HandlePickledJeffreyUserDMCommandAsync(SocketSlashCommand command)
        {
            var message = (string)command.Data.Options.First(m => m.Name == "message").Value;
            var user = (SocketUser)command.Data.Options.First(u => u.Name == "user").Value;

            await user.SendMessageAsync(message, TTSStateHandlerService.IsResponsesTts);
            await command.RespondAsync($"Sent DM to: {user.Username} - {message}");
        }

        [Command("poll")]
        private async Task HandlePollCommandAsync(SocketSlashCommand command)
        {
            //Poll command should take in a question to poll, and options for possible poll answers, which can be any length.
            //and contain whatever the user specifies as the message
        }

        [Command("8ball")]
        private async Task Handle8ballCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("tell_a_joke")]
        private async Task HandleTellJokeCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("meme")]
        private async Task HandleMemeCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("nsfw")]
        private async Task HandleNsfwCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("trivia")]
        private async Task HandleTriviaCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("rolladice")]
        private async Task HandleRolladiceCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("would_you_rather")]
        private async Task HandleWouldYouRatherCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("slap")]
        private async Task HandleSlapCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("hug")]
        private async Task HandleHugCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("quote_user")]
        private async Task HandleQuoteUserCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("rps")]
        private async Task HandleRpsCommandAsync(SocketSlashCommand command)
        {
            //Handle rock paper scissor game!
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("reverse_my_words")]
        private async Task HandleReverseWordsCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("give_me_a_fact")]
        private async Task HandleGiveMeFactCommandAsync(SocketSlashCommand command)
        {
            //If nothing is provided in the specific input for fact, it generates a completely random fact,
            //if something is provided, it generates a fact based on whats inputted.
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("high_five")]
        private async Task HandleHighFiveCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("insult")]
        private async Task HandleInsultCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("random_emoji")]
        private async Task HandleRandomEmojiCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("random_quote")]
        private async Task HandleRandomQuoteCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("server_latency")]
        private async Task HandleServerLatencyCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("mock_user")]
        private async Task HandleMockUserCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("remind_me")]
        private async Task HandleRemindMeCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("define_word")]
        private async Task HandleDefineWordCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("translate_to_language")]
        private async Task HandleTranslateToLanguageCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }
    }
}