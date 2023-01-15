using ApirooServer.AccountManagement;
using System.Net.Sockets;

namespace ApirooServer.Utils
{
    public static class Extensions
    {
        #region byte[] Extensions
        public static string PrintBytes(this byte[] buffer, int startIndex = 0, int? finalIndex = null)
        {
            string bytes = "";
            for (int i = (startIndex >= 1 && startIndex <= buffer.Length) ? startIndex + 1 : 1; finalIndex.HasValue ? finalIndex.Value > buffer.Length ? (i <= buffer.Length) : (i <= finalIndex) : (i <= buffer.Length); i++)
                bytes += buffer[i - 1].ToString("x").PadLeft(2, '0') + " ";
            return bytes;
        }
        #endregion
        #region String Extensions
        /// <summary> Cette extension permet de savoir si un string contient des charactères spécifiques </summary>
        /// <param name="str"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static bool ContainsChars(this string str, string toCompare = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            foreach (char c in str)
            {
                if (toCompare.Contains(c))
                    return true;
            }
            return false;
        }
        /// <summary> Cette extension permet de savoir si un string est un chiffre ou non de façons simple </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            foreach (char c in str)
            {
                if (!char.IsNumber(c))
                    return false;
            }
            return true;
        }
        #endregion
        #region TcpClient Extensions
        public static bool IsLogged(this TcpClient client) => AccountManager.Instance.GetAccount(client) != null;
        public static bool SocketConnected(this TcpClient client)
        {
            try
            {
                if (client != null && client.Client != null && client.Client.Connected)
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buffer = new byte[1];
                        if (client.Client.Receive(buffer, SocketFlags.Peek) == 0)
                            return false;
                        else
                            return true;
                    }
                    return true;
                }
                else
                    return false;
            }
            catch { return false; }
        }

        #endregion
    }
}
