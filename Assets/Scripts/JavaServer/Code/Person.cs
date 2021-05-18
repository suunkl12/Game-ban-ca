
using ProtoBuf;
using System.IO;

[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public int ID;
    [ProtoMember(2)]
    public string name;

    public byte[] Serialize()
    {
        byte[] msgOut;

        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, this);
            msgOut = stream.GetBuffer();
        }

        return msgOut;
    }
}
