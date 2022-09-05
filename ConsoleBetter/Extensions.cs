using System.Drawing;

namespace ConsoleBetter.Utils
{
    public static class Extensions
    {
        /// <summary> Cette extension permet de supprimer une séquence d'un string à partir d'une position et d'une taille </summary>
        /// <param name="from"></param>
        /// <param name="size"></param>
        public static string EraseSequence(this string str, Point from, Size size)
        {
            List<string> Lines = str.GetLines();
            size.Height = from.Y + size.Height;
            for (int i = from.Y; size.Height <= Lines.Count() ? i < size.Height : i < Lines.Count(); i++)
                Lines[i] = size.Width <= Lines[i].Length ? Lines[i].Remove(from.X, size.Width) : Lines[i] = string.Empty;
            str = string.Empty;
            foreach (string Line in Lines)
                str += Line != "\n" ? Line : string.Empty;
            return str;
        }
        /// <summary> Cette extension permet de récupérer toutes les lignes d'un string </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> GetLines(this string text)
        {
            List<string> Lines = new List<string>();
            string Line = string.Empty;
            foreach (char Char in text)
            {
                Line += Char;
                if (Char == '\n')
                {
                    Lines.Add(Line);
                    Line = string.Empty;
                }
            }
            if (Line.Trim() != string.Empty)
                Lines.Add(Line);
            return Lines;
        }
        public static string Append(this string str, int index, string toAppend)
        {
            string newStr = string.Empty;
            List<string> Lines = str.GetLines();
            if (index >= 0 && index <= Lines.Count)
                Lines.Insert(index, toAppend);
            else
                return toAppend;
            foreach (string Line in Lines)
                newStr += Line;
            return newStr;
        }


        #region StreamReaderExtensions
        public static byte[] ReadBytes(this StreamReader Reader, int count)
        {
            List<byte> readedBytes = new List<byte>();
            while (readedBytes.Count < count)
                readedBytes.Add((byte)Reader.Read());
            return readedBytes.ToArray();
        }
        #endregion
        #region byte[] Extenssions
        public static string PrintBytes(this byte[] buffer, int startIndex = 0, int? finalIndex = null)
        {
            string bytes = "";

            for (int i = (startIndex >= 1 && startIndex <= buffer.Length) ? startIndex + 1 : 1; finalIndex.HasValue ? finalIndex.Value > buffer.Length ? (i <= buffer.Length) : (i <= finalIndex) : (i <= buffer.Length); i++)
                bytes += buffer[i - 1].ToString("x").PadLeft(2, '0') + " ";
            return bytes;
        }
        #endregion
        #region List Extensions
        public static string ConvertString(this List<string> list)
        {
            string toReturn = string.Empty;
            for (int i = 0; i < list.Count; i++)
                toReturn += (i == list.Count - 1) ? $"{list[i]}" : $"{list[i]}\n";
            return toReturn;
        }
        #endregion
    }
}
