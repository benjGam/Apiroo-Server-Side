using ApirooServer.Utils;
using Console = ConsoleBetter.Console;

namespace ApirooServer.CommandManagement
{
    public class CommandManager : Singleton<CommandManager>
    {
        public static List<ICommand> CommandsQueue = new List<ICommand>();
        public CommandManager()
        {
            CommandsGetter.Init();
            new Logger<CommandManager>();
            new Thread(CommandInterpretor).Start();
        }
        private void CommandInterpretor()
        {
            while(Program.Running)
            {
                string userEntry = Console.ReadLine(Console.userAskingEntry);
                ICommand? Command = ICommand.Parse(userEntry.ToLower());
                if (Command == null)
                    Console.WriteLine($"La commande '{userEntry}' n'existe pas");
                else
                {
                    Command.Execute();
                    CommandsQueue.Add(Command);
                }
            }
        }

    }
}
