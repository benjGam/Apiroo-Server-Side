using System.Globalization;
using System.Net;
using System.Net.Sockets;
using ApirooServer.Utils;
using ApirooServer.Utils.MemoryBlock;

namespace ApirooServer.AccountManagement
{
    public class Account
    {
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string Pseudo { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public IPAddress? LastIP { get; private set; } = IPAddress.None;
        public DateTime? LastConnectionDate { get; private set; } = null;
        public DateTime? CreationDate { get; private set; } = null;
        public TcpClient ? Client { get; private set; } = null;
        public AccountType Type { get; private set; } = AccountType.Normal;
        public bool IsLogged => Client != null;
        public Account(BasicAccountMemoryBlock memoryBlock)
        {
            SetUsername(memoryBlock.GetSectionByKey("username").value);
            SetPassword(memoryBlock.GetSectionByKey("password").value);
            SetPseudo(memoryBlock.GetSectionByKey("pseudo").value);
            SetEmail(memoryBlock.GetSectionByKey("email").value);
            SetLastIP(memoryBlock.GetSectionByKey("lastIP").value);
            SetLastConnection(memoryBlock.GetSectionByKey("lastConnectionDate").value);
            SetType(memoryBlock.GetSectionByKey("type").value);
            SetCreationTime(memoryBlock.GetSectionByKey("creationDate").value);
        }

        #region Methods
        public void Bind(TcpClient client)
        {
            Client = client;
            SetLastConnection(DateTime.Parse(DateTime.Now.ToString(), new CultureInfo("fr-FR").DateTimeFormat));
            SetLastIP(IPAddress.Parse(client.Client.RemoteEndPoint!.ToString()!.Split(':')[0]));
            Logger<AccountManager>.Instance.Log($"{client.Client.RemoteEndPoint} binded on {Username} account", LogLevel.Info);
        }
        public void UnBind()
        {
            Client = null;
            Logger<AccountManager>.Instance.Log($"{Username} account disconnected", LogLevel.Info);
        }
        #endregion

        #region Setters
        public void SetUsername(string username) => Username = username;
        public void SetPassword(string password) => Password = password;
        public void SetPseudo(string pseudo) => Pseudo = pseudo;
        public void SetLastIP(IPAddress? ip = null) => LastIP = ip;
        public void SetLastIP(string ip = "") => LastIP = ip == "" ? IPAddress.None : IPAddress.Parse(ip);
        public void SetLastConnection(DateTime? lastConnectionDate) => LastConnectionDate = lastConnectionDate;
        public void SetLastConnection(string lastConnectionDate = "") => LastConnectionDate = lastConnectionDate == "" ? null : DateTime.Parse(lastConnectionDate, new CultureInfo("fr-FR").DateTimeFormat);
        public void SetCreationTime(DateTime? creationDate) => CreationDate = creationDate;
        public void SetCreationTime(string creationDate = "") => CreationDate = creationDate == "" ? null : DateTime.Parse(creationDate, new CultureInfo("fr-FR").DateTimeFormat);
        public void SetEmail(string email) => Email = email;
        public void SetClient(TcpClient? client) => Client = client;
        public void SetType(AccountType type) => Type = type;
        public void SetType(string type = "") => Type = type == "" ? AccountType.Normal : (AccountType)Enum.Parse(typeof(AccountType), type);
        #endregion
    }
}
