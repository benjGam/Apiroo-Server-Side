using System.Reflection;

namespace ApirooServer.CommandManagement
{
    public static class CommandsGetter
    {
        private static Dictionary<string, Type> Commands = new Dictionary<string, Type>();
        private static List<ICommand> CommandsInstances = new List<ICommand>();
        public static void Init()
        {
            Assembly.GetExecutingAssembly().GetTypes().ToList().FindAll(T => T.Namespace != null && T.Namespace.Contains("ApirooServer.CommandManagement.Commands") && T.GetProperty("cmd") != null).ForEach(T => Commands.Add((string)T.GetProperty("cmd")!.GetValue(Activator.CreateInstance(T)!)!, T));
            foreach (string cmdName in Commands.Keys)
                CommandsInstances.Add(GetInstance<ICommand>(cmdName)!);
        }
        public static T? GetInstance<T>(string cmd) where T : class => Commands.ContainsKey(cmd) ? Activator.CreateInstance(Commands[cmd]) as T : null;

        public static ICommand? GetCommand(string cmdName) => CommandsInstances.Find(command => command.cmd == cmdName);
        public static List<ICommand> GetAllCommands() => CommandsInstances;
    }
}
