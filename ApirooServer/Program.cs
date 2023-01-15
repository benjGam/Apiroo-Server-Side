using ApirooServer.AccountManagement;
using ApirooServer.PacketManagement;
using ApirooServer.CommandManagement;
using ApirooServer.ServerManagement;

namespace ApirooServer
{
    public static class Program
    {
        public static bool Running = true;
        public static void Main(string[] args)
        {
            new AccountManager();
            new FileManager();
            new PacketManager();
            new Server();
            new CommandManager();
        }
    }
}
