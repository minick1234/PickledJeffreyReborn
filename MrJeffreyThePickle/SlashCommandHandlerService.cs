using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MrJeffreyThePickle
{
    class SlashCommandHandlerService
    {
        private Dictionary<string, Func<SocketSlashCommand, Task>>? _commands;

        public SlashCommandHandlerService(AdminCommandHandler adminCommandHandler,
            GeneralCommandHandler generalCommandHandler, ChatGPTCommandHandlerService chatGPTCommandHandlerService)
        {
            _commands = new Dictionary<string, Func<SocketSlashCommand, Task>>();

            List<IRegisterSlashCommands> commandHandlers = new List<IRegisterSlashCommands>();
            commandHandlers.Add(adminCommandHandler);
            commandHandlers.Add(generalCommandHandler);
            commandHandlers.Add(chatGPTCommandHandlerService);

            foreach (var handler in commandHandlers)
            {
                //Grab all the methods from the command handler, which are public, an instance, or private.
                //Then filter it to grab the ones that include a custom attribute of type CommandAttribute.
                var methods = handler.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttribute<CommandAttribute>() != null);


                //For the methods that we found, we need to now filter out the command name from the CommandAttribute class to know,
                //which command links to what method. When we have that we can make a delegate to store that function. 
                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute<CommandAttribute>();
                    var commandName = attribute._CommandName;
                    Console.WriteLine(commandName);
                    Func<SocketSlashCommand, Task> commandDelegate = (Func<SocketSlashCommand, Task>)Delegate
                        .CreateDelegate(typeof(Func<SocketSlashCommand, Task>), handler, method);
                    _commands[commandName] = commandDelegate;
                }
            }
        }

        public async Task HandleCommandAsync(SocketSlashCommand command)
        {
            if (_commands.TryGetValue(command.Data.Name, out var commandHandler))
            {
                await commandHandler(command);
            }
            else
            {
                await command.RespondAsync("This is a unknown command kid.");
            }
        }
    }
}