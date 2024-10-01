using Discord.Commands;
using Discord.WebSocket;

namespace MrJeffreyThePickle;

public class RequireAdmin : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        var user = context.User as SocketGuildUser;
        if (user.GuildPermissions.Administrator)
        {
            return Task.FromResult(PreconditionResult.FromSuccess());
        }

        return Task.FromResult(PreconditionResult.FromError("You do not have permission to run this command."));
    }
}