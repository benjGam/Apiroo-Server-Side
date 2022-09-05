namespace ApirooServer.Utils.MemoryBlock
{
    public class BasicAccountMemoryBlock : MemoryBlock
    {
        protected new Dictionary<string, string> _keyPairs = new Dictionary<string, string>()
        {
            {"creationDate", string.Empty},
            {"username", string.Empty},
            {"password", string.Empty},
            {"pseudo", string.Empty},
            {"lastIP", string.Empty},
            {"lastConnectionDate", string.Empty},
            {"email", string.Empty},
            {"type", string.Empty},
        };
        protected override string _name => "Account";

    }
}
