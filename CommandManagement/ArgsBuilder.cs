using ApirooServer.Utils;

namespace ApirooServer.CommandManagement
{
    public static class ArgsBuilder
    {
        public const string noArgsTrame = "void";
        public static string[] Build(string Entry)
        {
            List<string> args = new List<string>();
            if (Entry.ContainsChars(" "))
            {
                string[] splittedUserEntry = Entry.Split(' ');
                foreach (string potentialArgument in splittedUserEntry)
                {
                    if ((potentialArgument.ContainsChars() || potentialArgument.ContainsChars("0123456789")) && potentialArgument != splittedUserEntry[0])
                        args.Add(potentialArgument);
                }
            }
            return args.ToList().ToArray();
        }
    }
}
