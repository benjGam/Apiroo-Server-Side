using Console = ConsoleBetter.Console;

namespace ApirooServer.CommandManagement.Commands.Global
{
    public class ViewService : ICommand
    {
        public string cmd => "view";

        public string Description => "Cette commande permet de chosir l'affichage du service concerné";

        public string Usage => "view [service]";

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
                
            }
            else
                Console.WriteLine("View command need a service name");
        }
    }
}
