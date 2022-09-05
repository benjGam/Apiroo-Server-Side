namespace ApirooServer.PacketManagement.IO
{
    public class IDataReader : IDisposable
    {
        private List<byte> Buffer = new List<byte>();

        public int Position { get; private set; } = 0;

        public IDataReader(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                Buffer.Add(buffer[i]);
        }

        public int ReadInt()
        {
            int readedInt = BitConverter.ToInt32(Buffer.ToArray(), Position);
            Position += sizeof(int);
            return readedInt;
        }
        public uint ReadUInt()
        {
            uint readedUInt = BitConverter.ToUInt32(Buffer.ToArray(), Position);
            Position += sizeof(uint);
            return readedUInt;
        }
        public short ReadShort()
        {
            short readedShort = BitConverter.ToInt16(Buffer.ToArray(), Position);
            Position += sizeof(short);
            return readedShort;
        }
        public ushort ReadUShort()
        {
            ushort readedUShort = BitConverter.ToUInt16(Buffer.ToArray(), Position);
            Position += sizeof(ushort);
            return readedUShort;
        }
        public char ReadChar()
        {
            char readedChar = (char)Buffer[Position];
            Position++;
            return readedChar;
        }
        public byte ReadByte()
        {
            byte readedByte = Buffer[Position];
            Position++;
            return readedByte;
        }
        public byte[] ReadBytes(int count)
        {
            byte[] toReturn = new byte[count];
            for(int i = 0; i < count; i++)
                toReturn[i] = ReadByte();
            return toReturn;
        }
        public string ReadString()
        {
            int Length = ReadInt();
            string readedString = string.Empty;
            for (int i = 0; i < Length; i++)
                readedString += ReadChar();
            return readedString;
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
