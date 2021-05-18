using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Message 
{
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
    public string Deserialize<T>(byte[] msgIn)
    {
        
        using (var stream = new MemoryStream())
        {
            Serializer.Deserialize(stream,msgIn);
            return stream.ToString();
        }
        
    }
}
