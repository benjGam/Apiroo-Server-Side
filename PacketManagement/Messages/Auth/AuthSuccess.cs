using ApirooServer.PacketManagement.IO;
using System.Globalization;

namespace ApirooServer.PacketManagement.Messages.Auth
{
    public class AuthSuccess : IPacket
    {
        public int ProtocolUUID => 2204;

        public string Username { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public string LastIP { get; set; } = string.Empty;
        public string LastConnectionDate { get; set; } = string.Empty;

        public AuthSuccess() { }
        public AuthSuccess(AccountManagement.Account account)
        {
            Username = account.Username;
            Pseudo = account.Pseudo;
            LastIP = account.LastIP!.ToString();
            LastConnectionDate = account.LastConnectionDate!.Value.ToString(new CultureInfo("fr-FR").DateTimeFormat);
        }
        public void Deserialize(ref IDataReader dataOutput)
        {
            Username = dataOutput.ReadString();
            Pseudo = dataOutput.ReadString();
            LastIP = dataOutput.ReadString();
            LastConnectionDate = dataOutput.ReadString();
        }
        public void Serialize(ref IDataWriter dataInput)
        {
            dataInput.WriteString(Username);
            dataInput.WriteString(Pseudo);
            dataInput.WriteString(LastIP);
            dataInput.WriteString(LastConnectionDate);
        }
    }
}
