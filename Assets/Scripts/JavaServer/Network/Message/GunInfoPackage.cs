using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public class GunInfoPackage : Message
{
    public GunInfoPackage()
    {
    }

    public override void Read()
    {
        int id = (int)objects[0];

        //Trong dictinary không có cái key nào trùng thì không dọc dữ liệu gửi đến
        if (!Client.instance.guns.ContainsKey(id)) return;

    }

    public override void Write()
    {
        throw new System.NotImplementedException();
    }
}
