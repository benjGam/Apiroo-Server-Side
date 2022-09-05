using ApirooServer.Utils;

namespace ApirooServer.CommandManagement
{
    public interface ICommand
    {
        public string cmd { get; }
        public string Description { get; }
        public string Usage { get; }
        public string[] Args { get; set; }
        protected abstract string[] _args { get; set; }
        public static ICommand? Parse(string userEntry)
        {
            ICommand? command = null;
            if(userEntry.ContainsChars())
            {
                string[] args = ArgsBuilder.Build(userEntry);
                if (args.Length > 0)
                {
                    command = CommandsGetter.GetInstance<ICommand>(userEntry.Split(' ')[0])!;
                    if (command != null)
                        command.Args = args;
                }
                else
                {
                    command = CommandsGetter.GetInstance<ICommand>(userEntry);
                    if (command != null)
                        command.Args = args;
                }
            } 
            return command;
        }
        public string Infos => $"{cmd}: {Description} (Usage: {Usage})";
        public void Execute();
    }
}
