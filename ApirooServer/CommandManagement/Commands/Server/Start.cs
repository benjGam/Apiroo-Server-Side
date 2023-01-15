using ApirooServer.Utils;

namespace ApirooServer.CommandManagement.Commands.Server
{
    public class Start : ICommand
    {
        public string cmd => "start";
        public string Description => "Cette commande permet de démarrer le serveur";
        public string Usage => $"{cmd} [optionnal: port]";
        public string[] _args { get; set; } = new string[1] ;
        public string[] Args  
        { 
            get => _args; 
            set
            {
                int port = ServerManagement.Server.Instance.Port;
                if (value.Length > 0)
                    _args[0] = value.ToList().Find(arg => arg.IsNumber()) ?? port.ToString();
                else
                    _args[0] = port.ToString();
            }
        }
        public void Execute() => ServerManagement.Server.Instance.Start(int.Parse(Args[0]));
    }
}
