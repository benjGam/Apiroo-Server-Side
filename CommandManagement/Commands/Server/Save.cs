using ApirooServer.Utils;

namespace ApirooServer.CommandManagement.Commands.Server
{
    public class Save : ICommand
    {
        public string cmd => "save";
        public string Description => "Cette commande permet de démarrer le serveur";
        public string Usage => $"{cmd} [optionnal: port]";
        public string[] _args { get; set; } = new string[0];
        public string[] Args
        {
            get => _args;
            set => _args = new string[0];
        }
        public void Execute()
        {
            //Saving
        }
    }
}
