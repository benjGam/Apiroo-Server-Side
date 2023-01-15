using Console = ConsoleBetter.Console;

namespace ApirooServer.CommandManagement.Commands.Global
{
    public class Test : ICommand
    {
        public string cmd => "test";
        public string Description => "Permet de tester du code";
        public string Usage => "test";
        public string[] _args { get; set; } = new string[0];
        public string[] Args 
        { 
            get => _args; 
            set => _args = new string[0];
        }

        public void Execute()
        {
            
        }
    }
}
