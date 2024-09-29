using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrJeffreyThePickle
{
    internal class AdminCommandHandler : IRegisterSlashCommands
    {
        private readonly DiscordSocketClient _client;

        public AdminCommandHandler(DiscordSocketClient client) { _client = client; }

        [Command("ban")]
        private async Task HandleBanUser(SocketSlashCommand command)
        {
            await command.RespondAsync("pretend banning a user.");
        }

    }
}
