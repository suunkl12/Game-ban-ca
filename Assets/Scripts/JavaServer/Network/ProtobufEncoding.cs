using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ProtobufEncoding
{

    public static byte[] Encode(Google.Protobuf.IMessage msg, bool b)
    {
        using (MemoryStream rawOutput = new MemoryStream())
        {
            CodedOutputStream output = new CodedOutputStream(rawOutput);

            //WriteRawVarint32 is deprecated
            if (b) output.WriteUInt32((uint)msg.CalculateSize() - 1);
            output.WriteMessage(msg);
            output.Flush();
            byte[] result = rawOutput.ToArray();

            return result;
        }
    }

    public static KeyValuePair<byte[], List<Google.Protobuf.IMessage>> Decode(byte[] msg)
    {
        /// List của những message nhận được
        List<Google.Protobuf.IMessage> list = new List<Google.Protobuf.IMessage>();

        CodedInputStream stream = new CodedInputStream(msg);
        
        //???
        bool atkat = false;
        
        int varint32 = 0;

        while (true)
        {
            if (stream.IsAtEnd) break;

            try
            {
                varint32 = (int)stream.ReadRawVarint32();
                if (varint32 <= (msg.Length - (int)stream.Position))
                {
                    byte[] body = stream.ReadRawBytes(varint32);


                    //Thêm message của packet nhận được vào list
                    list.Add(Packet.Parser.ParseFrom(body));
                }
                else
                {
                    atkat = true;
                    break;
                }

            }
            catch
            {

                break;

            }

        }

        byte[] bytes;
        if (stream.IsAtEnd) bytes = new byte[0];
        else
        {
            if (atkat)
            {
                byte[] b = stream.ReadRawBytes(msg.Length - (int)stream.Position);
                bytes = new byte[b.Length + 4];
                byte[] intBytes = BitConverter.GetBytes(varint32);
                Array.Reverse(intBytes);
                Array.Copy(intBytes, bytes, intBytes.Length);
                Array.Copy(b, 0, bytes, intBytes.Length, b.Length);
            }
            else bytes = stream.ReadRawBytes(msg.Length - (int)stream.Position);
        }

        return new KeyValuePair<byte[], List<Google.Protobuf.IMessage>>(bytes, list);
    }

}