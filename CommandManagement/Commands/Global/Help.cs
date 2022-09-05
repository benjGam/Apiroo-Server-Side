using Console = ConsoleBetter.Console;

namespace ApirooServer.CommandManagement.Commands.Global
{
    public class Help : ICommand
    {
        public string cmd => "help";
        public string Description => "Permet d'obtenir de l'aide";
        public string Usage => "help [optionnal: cmd]";
        public string[] _args { get; set; } = new string[1];
        public string[] Args 
        { 
            get => _args; 
            set => _args[0] = value.Length > 0 ? value.First() : ArgsBuilder.noArgsTrame;
        }

        public void Execute()
        {
            if (_args[0] != ArgsBuilder.noArgsTrame)
            {
                ICommand? requestedCommand = CommandsGetter.GetCommand(_args[0].ToLower());
                Console.WriteLine(requestedCommand != null ? requestedCommand.Infos! : $"Aucune aide trouvéeé pour la commande '{_args[0]}'");
            }
            else
            {
                string totalHelpTrame = string.Empty;
                foreach (ICommand command in CommandsGetter.GetAllCommands())
                    totalHelpTrame += $"{(totalHelpTrame.Length == 0 ? command.Infos : $"\n{command.Infos}")}";
                Console.WriteLine(totalHelpTrame);
            }
        }
    }
}
