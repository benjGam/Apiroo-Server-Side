using ApirooServer.PacketManagement.IO;

namespace ApirooServer.PacketManagement.Messages.Auth
{
    public class AuthRequest : IPacket
    {
        //Client packet (Never initialized, serialized)
        public int ProtocolUUID => 2202;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;   
        public void Deserialize(ref IDataReader dataOutput)
        {
            Username = dataOutput.ReadString();
            Password = dataOutput.ReadString();
        }
        public void Serialize(ref IDataWriter dataInput)
        {
            dataInput.WriteString(Username);
            dataInput.WriteString(Password);
        }
    }
}
