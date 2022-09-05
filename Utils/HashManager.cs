using System.Security.Cryptography;
using System.Text;

namespace ApirooServer.Utils
{
    public static class HashManager
    {
        private static SHA256 Hasher = SHA256.Create();
        public static string Hash(string strToHash)
        {
            byte[] data = Hasher.ComputeHash(Encoding.UTF8.GetBytes(strToHash));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
        public static bool Compare(string strToVerify, string hashedString) => StringComparer.OrdinalIgnoreCase.Compare(strToVerify, hashedString) == 0;
    }
}
