using System.Drawing;
using ConsoleBetter.Utils;
using SysConsole = System.Console;

namespace ConsoleBetter
{
    public static class Console
    {
        private static string textBuffer = string.Empty;
        private static TextWriter OutReader = System.Console.Out;
        public static string userAskingEntry = "> ";
        #region Methods

        public static void Write(string output)
        {
            AppendBuffers(output, true);
        }
        public static void WriteLine(string output, bool push = true)
        {
            AppendBuffers($"{output}\n", push == false ? textBuffer.GetLines().Count >= 2 ? false : true : true); //Si push est faux, on verifie si y'a la place pour append, si y'a pas la place on envoi false donc 
        }
        private static void ProcessOut()
        {
            List<string> Lines = textBuffer.GetLines();
            System.Console.SetCursorPosition(0, Lines.Count() - 2);
            OutReader.Write(GetBlankSequence(Lines.Last().Length));
            System.Console.SetCursorPosition(0, Lines.Count() - 2);
            OutReader.Write(Lines[Lines.Count() - 2]);
            OutReader.Write(Lines.Last());
        }
        private static void AppendBuffers(string formatedOutput, bool push)
        {
            if (push)
            {
                textBuffer += formatedOutput;
                OutReader.Write(formatedOutput);
            }
            else
            {
                textBuffer = textBuffer.Append(textBuffer.GetLines().Count - 1, formatedOutput);
                ProcessOut();
            }
        }
        private static string GetBlankSequence(int sequenceSize)
        {
            string blankSequence = string.Empty;
            for (int i = 0; i < sequenceSize; i++)
                blankSequence += ' ';
            return blankSequence;
        }
        public static void EraseSequence(Point sequenceLocation, Size sequenceSize)
        {
            for (int i = 0; i < sequenceSize.Height; i++)
            {
                System.Console.SetCursorPosition(sequenceLocation.X, sequenceLocation.Y + i); //Setting cursor to sequence to erase
                OutReader.Write(GetBlankSequence(sequenceSize.Width)); //Blanking sequence from visible console buffer
            }
            System.Console.SetCursorPosition(sequenceLocation.X, sequenceLocation.Y);
            textBuffer = textBuffer.EraseSequence(sequenceLocation, sequenceSize);
        }
        public static string ReadLine(string text = "", char charBuffer = '\0')
        {
            Write(text);
            bool Continue = true;
            List<char> chars = new List<char>();
            while (Continue)
            {
                ConsoleKeyInfo KeyInfo = System.Console.ReadKey(true);
                switch (KeyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        Write("\n");
                        Continue = false;
                        break;
                    case ConsoleKey.Backspace:
                        if (chars.Count - 1 >= 0)
                        {
                            EraseSequence(new Point(System.Console.CursorLeft - 1, System.Console.CursorTop), new Size(1, 1));
                            chars.RemoveAt(chars.Count - 1);
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        chars.Add(' ');
                        Write(" ");
                        break;
                    default:
                        if (!KeyInfo.Key.Equals(ConsoleKey.LeftArrow) && !KeyInfo.Key.Equals(ConsoleKey.RightArrow) && !KeyInfo.Key.Equals(ConsoleKey.UpArrow) && !KeyInfo.Key.Equals(ConsoleKey.DownArrow))
                        {
                            chars.Add(KeyInfo.KeyChar);
                            Write(charBuffer == '\0' ? chars.Last().ToString() : charBuffer.ToString());
                        }
                        break;
                }
            }
            string toReturn = string.Empty;
            foreach (char c in chars) { toReturn += c; }
            return toReturn;
        }
        #endregion
    }
}
