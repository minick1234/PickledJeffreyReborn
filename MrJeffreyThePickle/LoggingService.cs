using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrJeffreyThePickle
{
    class LoggingService
    {
        public LoggingService()
        {

        }

        public Task LogAsync(LogMessage message)
        {
            if (message.Exception is CommandException cmdException)
            {
                Console.WriteLine($"[Command:{message.Severity}] {cmdException.Command.Aliases.First()} Failed to execute in: {cmdException.Context.Channel}");
            }
            else
            {
                Console.WriteLine($"[General:{message.Severity}] {message}");
            }

            return Task.CompletedTask;
        }
    }
}
