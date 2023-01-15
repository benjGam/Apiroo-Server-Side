using ApirooServer.PacketManagement.IO;

namespace ApirooServer.PacketManagement.Messages.Auth
{
    public class AuthError : IPacket
    {
        public int ProtocolUUID => 2203;

        public string Message { get; set; } = string.Empty;

        public AuthError() { }
        public AuthError(string message)
        {
            Message = message;
        }
        public void Deserialize(ref IDataReader dataOutput)
        {
            Message = dataOutput.ReadString();
        }
        public void Serialize(ref IDataWriter dataInput)
        {
            dataInput.WriteString(Message);
        }
    }
}
