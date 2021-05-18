
using ProtoBuf;
using System.IO;

[ProtoContract]
public class Person:Message
{
    [ProtoMember(1)]
    public int ID;

    [ProtoMember(2)]
    public string name;
}
