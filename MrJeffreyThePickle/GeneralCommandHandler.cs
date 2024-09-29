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
        private readonly TTSStateHandlerService _ToggleTTSService;

        public GeneralCommandHandler(DiscordSocketClient client, TTSStateHandlerService toggleTTSService)
        {
            _client = client;
            _ToggleTTSService = toggleTTSService;
        }

        [Command("tts_toggle")]
        private async Task HandleTTSToggleAsync(SocketSlashCommand command)
        {
            _ToggleTTSService.IsResponsesTts = !_ToggleTTSService.IsResponsesTts;
            if (_ToggleTTSService.IsResponsesTts)
            {
                await command.RespondAsync("TTS is enabled", null, _ToggleTTSService.IsResponsesTts);
            }
            else
            {
                await command.RespondAsync("TTS is disabled", null, _ToggleTTSService.IsResponsesTts);
            }
        }

        [Command("ping")]
        private async Task HandlePingCommandAsync(SocketSlashCommand command)
        {
            await command.RespondAsync("PONG MY BOY", null, _ToggleTTSService.IsResponsesTts);
        }

        [Command("echo")]
        private async Task HandleEchoCommandAsync(SocketSlashCommand command)
        {
            var message = (string)command.Data.Options.First(m => m.Name == "message").Value;

            await command.RespondAsync(message, null, _ToggleTTSService.IsResponsesTts);
        }

        [Command("pickledjeffrey_dm_user")]
        private async Task HandlePickledJeffreyUserDMCommandAsync(SocketSlashCommand command)
        {
            var message = (string)command.Data.Options.First(m => m.Name == "message").Value;
            var user = (SocketUser)command.Data.Options.First(u => u.Name == "user").Value;
            
            await user.SendMessageAsync(message, _ToggleTTSService.IsResponsesTts);
            await command.RespondAsync($"Sent DM to: {user.Username} - {message}");
        }
    }
}