namespace ApirooServer.CommandManagement.Commands.Server
{
    public class Stop : ICommand
    {
        public string cmd => "stop";
        public string Description => "Cette commande permet de fermer le serveur";
        public string Usage =>  $"{cmd} [optionnal: reason]";
        public string[] _args { get; set; } = new string[1];
        public string[] Args 
        {  
            get => _args; 
            set => _args[0] = value.Length > 0 ? string.Join(" ", value) : ArgsBuilder.noArgsTrame;
        }
        public void Execute() => ServerManagement.Server.Instance.Stop();
    }
}
