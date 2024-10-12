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

        public AdminCommandHandler(DiscordSocketClient client)
        {
            _client = client;
        }
        
        [Command("ban")]
        private async Task HandleBanUser(SocketSlashCommand command)
        {
            var user = (SocketUser)command.Data.Options.First(n => n.Name == "user").Value;
            var reason = (string)command.Data.Options.First(n => n.Name == "reason").Value;

            //Figure this out -- var length = command.Data.Options.First(n => n.Name == "length").Value;
            //Figure this out -- var delete_messages = command.Data.Options.First(n => n.Name == "delete_messages").Value;

            string dmBanMessage =
                $"Uh oh, {user.Mention}! The supreme leader's have spoken, and it looks like you've been officially promoted to 'Ex-Guild Member' status in {_client.GetGuild(command.GuildId.Value).Name}🏴‍☠️\n" +
                $"Reason: {reason ?? "Being a menace to society, and wreaking havoc in our lands."} \n\n" +
                $"Enjoy your ban-cation! Don't worry, we’ll send you a postcard✌️";


            await user.SendMessageAsync(dmBanMessage, TTSStateHandlerService.IsResponsesTts);
            await _client.GetGuild(command.GuildId.Value).BanUserAsync(user);
            await command.RespondAsync($"Banned User: {user}, reason: {reason}", null,
                TTSStateHandlerService.IsResponsesTts);
        }

        [Command("unban")]
        private async Task HandleUnbanUser(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        private async Task CheckUserBannedTime()
        {
            //Check how long a user is banned for from the database, when this happens, we want to unban them and then resend
            //them an invite to the guild they are banned from.
        }

        [Command("kick")]
        private async Task HandleKickUser(SocketSlashCommand command)
        {
            //Grab the user to kick
            var user = (SocketUser)command.Data.Options.First(n => n.Name == "user").Value;
            var reason = (string)command.Data.Options.First(n => n.Name == "reason").Value;

            await user.SendMessageAsync(
                $"**Unfortunately friend you have been kicked from the server for the following reason:**\n\n{reason}",
                TTSStateHandlerService.IsResponsesTts);
            await _client.GetGuild((ulong)command.GuildId).GetUser(user.Id).KickAsync(reason);
            await command.RespondAsync($"**Kicked {user.Username} from the server.**\\n**Reason:**\n{reason}");
        }

        [Command("clear_channel_messages")]
        private async Task ClearChatMessages(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("change_user_role")]
        private async Task ChangeUsersRole(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("admin_mute_user")]
        private async Task AdminMuteUser(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("admin_unmute_user")]
        private async Task AdminUnmuteUser(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("banned_user_list")]
        private async Task ListBannedUsers(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("lock_channel")]
        private async Task LockChannel(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("unlock_channel")]
        private async Task UnlockChannel(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }

        [Command("warn_user")]
        private async Task WarnUser(SocketSlashCommand command)
        {
            await command.RespondAsync("TTS is disabled", null, TTSStateHandlerService.IsResponsesTts);
        }
    }
}