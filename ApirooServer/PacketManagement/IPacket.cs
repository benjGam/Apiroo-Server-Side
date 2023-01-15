using ApirooServer.PacketManagement.IO;

namespace ApirooServer.PacketManagement
{
    public interface IPacket
    {
        public int ProtocolUUID { get; }
        public void Deserialize(ref IDataReader dataOutput);
        public void Serialize(ref IDataWriter dataInput);
        public byte[] Pack()
        {
            IDataWriter datasWriter = new IDataWriter();
            Serialize(ref datasWriter);
            return datasWriter.Stream;
        }
        public void Unpack(byte[] datas)
        {
            IDataReader reader = new IDataReader(datas);
            Deserialize(ref reader);
        }
    }
}
