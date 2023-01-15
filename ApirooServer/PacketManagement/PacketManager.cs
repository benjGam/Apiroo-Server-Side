using ApirooServer.PacketManagement.IO;
using ApirooServer.PacketManagement.Messages.Auth;
using ApirooServer.ServerManagement;
using ApirooServer.AccountManagement;
using ApirooServer.Utils;
using System.Net.Sockets;
using System.Reflection;
using Console = ConsoleBetter.Console;

namespace ApirooServer.PacketManagement
{
    public class PacketManager : Singleton<PacketManager>
    {
        private Dictionary<int, Type> PacketsClass = new Dictionary<int, Type>();
        public PacketManager()
        {
            new Logger<PacketManager>();
            InitPacketsClass();
        }

        #region Setup Initator Service
        private void InitPacketsClass()
        {
            Assembly.GetExecutingAssembly().GetTypes().ToList().FindAll(T => T.Namespace != null && T.Namespace.Contains("ApirooServer.PacketManagement.Messages") && T.GetProperty("ProtocolUUID") != null).ForEach(T => PacketsClass.Add((int)T.GetProperty("ProtocolUUID")!.GetValue(Activator.CreateInstance(T)!)!, T));
            Logger<PacketManager>.Instance.Log($"{PacketsClass.Count} packets class getted", LogLevel.Info);
        }
        private T? GetPacketInstance<T>(int protocolUIID) where T : class => PacketsClass.ContainsKey(protocolUIID) ? Activator.CreateInstance(PacketsClass[protocolUIID]) as T : null;
        #endregion

        #region Methods

        private ushort Compute_Static_Header(int messageId, ushort typedLength) => (ushort)((messageId << 3) | typedLength);
        private static ushort Len(int len)
        {
            if (len > 65535)
                return 3;
            if (len > 255)
                return 2;
            if (len > 0)
                return 1;
            return 0;
        }
        public void Recieve(TcpClient client, byte[] buffer)
        {
            List<byte> instanciedBuffer = buffer.ToList();
            do
            {
                IDataReader reader = new IDataReader(instanciedBuffer.ToArray());
                ushort Header = reader.ReadUShort();
                ushort protocolId = (ushort)(Header >> 3);
                ushort compLength = (ushort)(Header & 3);
                ushort packetLength = 0;
                switch (compLength)
                {
                    case 1:
                        packetLength = reader.ReadByte();
                        break;
                    case 2:
                        packetLength = reader.ReadUShort();
                        break;
                    case 3:
                        packetLength = (ushort)reader.ReadInt();
                        break;
                }
                if (instanciedBuffer.Count >= packetLength) //All packet datas are in buffer (Normaly ALWAYS)
                {
                    byte[] bufferedBytes = reader.ReadBytes(packetLength);
                    instanciedBuffer.RemoveRange(0, reader.Position);
                    IPacket? recievedPacket = GetPacketInstance<IPacket>(protocolId);
                    if (recievedPacket != null)
                        recievedPacket.Unpack(bufferedBytes);
                    Dispatch(recievedPacket, client, protocolId);
                }
            } while (instanciedBuffer.Count > 0);
        }
        public void Send(TcpClient client, IPacket packet)
        {
            IDataWriter Writer = new IDataWriter();
            byte[] datas = packet.Pack();
            ushort Length = Len(datas.Length);
            Writer.WriteUShort(Compute_Static_Header(packet.ProtocolUUID, Length));
            switch(Length)
            {
                case 1:
                    Writer.WriteByte((byte)datas.Length);
                    break;
                case 2:
                    Writer.WriteUShort((ushort)datas.Length);
                    break;
                case 3:
                    Writer.WriteInt(datas.Length);
                    break;
            }
            Writer.WriteBytes(datas);
            Server.Instance.Send(client, Writer.Stream);
        }

        #endregion

        #region Dispatcher Service
        private void Dispatch(IPacket? recievedPacket, TcpClient client, int recievedPacketId)
        {
            switch (recievedPacket)
            {
                case AuthRequest:
                    AuthPacket_Recieved(client, (AuthRequest)recievedPacket);
                    break;
                case null:
                    Logger<PacketManager>.Instance.Log($"{client.Client.RemoteEndPoint} sent {recievedPacketId} packet ID.", LogLevel.Error);
                    Server.Instance.Disconnect_Client(client, "Sent invalid packet");
                    break;
                default:
                    Logger<PacketManager>.Instance.Log($"{client.Client.RemoteEndPoint} sent unmanaged packet (ID: {recievedPacketId})", LogLevel.Error);
                    Console.WriteLine($"{client.Client.RemoteEndPoint} sent unmanaged packet (ID: {recievedPacketId})", false);
                    break;
            }
        }
        private void AuthPacket_Recieved(TcpClient client, AuthRequest packet)
        {
            if (!client.IsLogged())
            {
                Account? accountToSearch = AccountManager.Instance.GetAccount(packet.Username);
                if (accountToSearch != null)
                {
                    if (!accountToSearch.IsLogged)
                    {
                        if (HashManager.Compare(accountToSearch.Password, packet.Password))
                        {
                            Send(client, new AuthSuccess(accountToSearch));
                            accountToSearch.Bind(client);
                        }
                        else
                            Send(client, new AuthError("Verifiez les identifiants"));
                    }
                    else
                        Send(client, new AuthError("Quelqu'un est déjà connecté sur ce compte"));
                }
                else
                    Send(client, new AuthError($"Ce  compte n'existe pas"));
            }
        }
       
        #endregion
    }
}

//CHiffrer les paquets au moment de l'envoi (Pour ne pas avoir d'info en clair et qu'on puisse pas voir les auth request avec les hashs dedans par exemple)
//Client & Server side, [IMPORTANT]
