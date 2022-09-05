namespace ApirooServer.PacketManagement.IO
{
    public class IDataWriter : IDisposable
    {
        private List<byte> Buffer = new List<byte>();
        public int Position { get; private set; } = 0;
        public IDataWriter(byte[]? previousBuffer = null)
        {
            if(previousBuffer != null)
            {
                foreach(byte Byte in previousBuffer)
                {
                    Buffer.Add(Byte);
                    Position++;
                }
            }
        }
        public byte[] Stream => Buffer.ToArray();
        public void WriteInt(int toWrite)
        {
            Buffer.AddRange(BitConverter.GetBytes(toWrite));
            Position += sizeof(int);
        }
        public void WriteUInt(uint toWrite)
        {
            Buffer.AddRange(BitConverter.GetBytes(toWrite));
            Position += sizeof(uint);
        }
        public void WriteShort(short toWrite)
        {
            Buffer.AddRange(BitConverter.GetBytes(toWrite));
            Position += sizeof(short);
        }
        public void WriteUShort(ushort toWrite)
        {
            Buffer.AddRange(BitConverter.GetBytes(toWrite));
            Position += sizeof(ushort);
        }
        public void WriteChar(char toWrite)
        {
            Buffer.Add((byte)toWrite);
            Position++;
        }
        public void WriteChar(byte toWrite)
        {
            Buffer.Add(toWrite);
            Position++;
        }
        public void WriteString(string toWrite)
        {
            int length = toWrite.Length;
            WriteInt(length);
            toWrite.ToList().ForEach(c => WriteChar(c));
        }
        public void WriteByte(byte toWrite)
        {
            Buffer.Add(toWrite);
            Position++;
        }
        public void WriteBytes(byte[] toWrite)
        {
            foreach (byte Byte in toWrite)
                WriteByte(Byte);
        }
        public void Close() => Dispose();
        public void Close(ref byte[] toSave)
        {
            toSave = Buffer.ToArray();
            Dispose();
        }
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
