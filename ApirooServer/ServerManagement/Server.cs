using ApirooServer.Utils;
using System.Net;
using System.Net.Sockets;
using ApirooServer.PacketManagement;
using Console = ConsoleBetter.Console;

namespace ApirooServer.ServerManagement
{
    public class Server : Singleton<Server>
    {
        private TcpListener _server = null!;
        public List<TcpClient> Clients = new List<TcpClient>();
        public int Port { get; private set; } = 5005;
        public bool IsStarted { get; private set; } = false;
        public const int MAX_MESSAGE_SIZE = 108 * 1024;

        public Server()
        {
            Console.WriteLine($"Apiroo Server v1.0");
            new Logger<Server>();
        }


        #region External Methods
        public void Start(int? port = null)
        {
            if(!IsStarted)
            {
                IsStarted = true;
                Port = port ?? Port;
                _server = new TcpListener(IPAddress.Parse("192.168.1.10"), Port);
                _server.Start();
                Console.WriteLine("Server starting");
                Logger<Server>.Instance.Log("Server starting", LogLevel.Info);
                new Thread(new ThreadStart(Listening)).Start();
            }
        }
        public void Stop()
        {
            if(IsStarted)
            {
                IsStarted = false;
                Console.WriteLine("Server stopped");
                Logger<Server>.Instance.Log("Server stopped", LogLevel.Info);
            }
        }
        public void Disconnect_Client(TcpClient client, string? reason = null) => Client_Disconnected(client, reason);
        #endregion
        #region Listener Management

        private void Listening()
        {
            Logger<Server>.Instance.Log("Start listening", LogLevel.Info);
            while (IsStarted)
            {
                if (_server.Pending()) //Client want to connect
                {
                    TcpClient requestingClient = _server.AcceptTcpClient();
                    Client_Connected(requestingClient);
                }
            }
            Logger<Server>.Instance.Log("Stop listening", LogLevel.Info);
            Clients.GetRange(0, Clients.Count).ForEach(Client => Disconnect_Client(Client));
            _server.Stop();
        }
        public void Data_Recieve(TcpClient client, byte[] buffer) => PacketManager.Instance.Recieve(client, buffer);
        public void Send(TcpClient client, byte[] buffer)
        {
            try
            {
                client.GetStream().Write(buffer);
            }
            catch { Disconnect_Client(client, "Cant hook client stream"); }
        }
        private void Client_Connected(TcpClient client)
        {
            Clients.Add(client);
            new Task(() => Client_Sent_Datas(client)).Start(); //Adding reading task for client
            Console.WriteLine($"Un nouveau client vient de se connecter : {client.Client.RemoteEndPoint}", false);
        }
        private void Client_Disconnected(TcpClient client, string? reason = null)
        {
            if (Clients.Contains(client))
            {
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected {(reason != null ? $"({reason})" : string.Empty)}", false);
                Logger<Server>.Instance.Log($"Client {client.Client.RemoteEndPoint} disconnected {(reason != null ? $"({reason})" : string.Empty)}", LogLevel.Info);
                if(client.IsLogged()) //Client was connected on account
                {
                    //Send all PLAYERS disconnectedPlayer message
                    AccountManagement.AccountManager.Instance.GetAccount(client)!.UnBind();
                }
                client.Close();
                Clients.Remove(client);
            }
        }
        private void Client_Sent_Datas(TcpClient client)
        {
            NetworkStream Stream = client.GetStream();
            while (client.SocketConnected()) //While client is REALY connected (See ApirooServer.Extensions at TcpClient Extensions)
            {
                try
                {
                    if (Stream.DataAvailable) //If client stream append some bytes
                    {
                        byte[] recievedBuffer = new byte[MAX_MESSAGE_SIZE];
                        int bytesReaded = Stream.Read(recievedBuffer, 0, recievedBuffer.Length);
                        if (bytesReaded > 0) //If recieved bytes len > 0
                        {
                            if (bytesReaded > MAX_MESSAGE_SIZE) //If recieved bytes len > MAX_MESSAGE_SIZE (128 * 1024)
                            {
                                Console.WriteLine($"Client {client.Client.RemoteEndPoint} sent {bytesReaded} bytes (Too large)");
                                break;
                            }
                            else //If recieved bytes len conform
                            {
                                Array.Resize(ref recievedBuffer, bytesReaded); //Adapt recievedBuffer size with exact recieved bytes len
                                Data_Recieve(client, recievedBuffer); //La fonction appeler (enfin celle dans le packetManager enfaite) devra être retravailler, Client et Server side pour s'adapter au fait que ce soit un flux de donnée constant finalement, (comme sur Dofus)
                            }
                        }
                        else //If recieved bytes len <= 0
                        {
                            Console.WriteLine($"Client {client.Client.RemoteEndPoint} sent 0 bytes (Useless, and non normal)");
                            break;
                        }
                    }
                    else //Client not sending anything
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                }
                catch { break; } //If stream is disposed or erorr occured when tryin to know if client stream data append some bytes
            }
            Disconnect_Client(client);
        }

        #endregion

    }
}
