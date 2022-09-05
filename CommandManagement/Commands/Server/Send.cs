using ApirooServer.PacketManagement.Messages.Auth;
using ApirooServer.Utils;

namespace ApirooServer.CommandManagement.Commands.Server
{
    public class Send : ICommand
    {
        public string cmd => "send";
        public string Description => "Cette commande permet d'envoyer un ping à un client";
        public string Usage => $"{cmd}";
        public string[] _args { get; set; } = new string[0];
        public string[] Args
        {
            get => _args;
            set => _args = new string[0];
        }
        public void Execute()
        {
            if (ApirooServer.ServerManagement.Server.Instance.Clients.Count > 0)
                PacketManagement.PacketManager.Instance.Send(ApirooServer.ServerManagement.Server.Instance.Clients.Last(), new AuthError("Je t'envois un ping petit client"));
            else
                Console.WriteLine("Y'a aucun client bougre d'idiot");
            //Saving
        }
    }
}
