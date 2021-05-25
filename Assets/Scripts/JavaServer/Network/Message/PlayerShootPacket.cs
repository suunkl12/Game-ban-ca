using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerShootPacket : MessagePacket
{
    static PlayerShootPacket(){
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType,
            new Type[]
            {
                typeof(int), // ID
                typeof(float) // bullet degree 
            });
    }

    public PlayerShootPacket(int id, float degree)
    {
        objects.Add(id);
        objects.Add(degree);
    }
    public PlayerShootPacket() { }

    public override void Read()
    {
        //Debug.Log("Player " + (int)objects[0] +" is firing");
        var degree = (float)objects[1];
        //throw new System.NotImplementedException();
        Client.instance.guns[(int)objects[0]].RpcShoot(degree);
    }

    public override void Write()
    {
        Client.instance.Send(this);
    }
}
