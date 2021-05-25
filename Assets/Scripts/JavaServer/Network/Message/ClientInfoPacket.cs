using System;
using System.Reflection;
using UnityEngine;

public class ClientInfoPacket : MessagePacket
{

    static ClientInfoPacket()
    {
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType, new Type[] {
            typeof(int), // ID
            typeof(int) // Objects number
        });
    }

    public ClientInfoPacket() { }

    public ClientInfoPacket(int i)
    {

        objects.Add(i);

    }

    public override void Read()
    {
        Debug.Log("Client info receive");
        Client.instance.id = (int)objects[0];
        if (Client.instance.guns.ContainsKey((int)Client.instance.id))
        {

            GunController pc = Client.instance.guns[(int)Client.instance.id];
            pc.controllable = true;
            //pc.Start();
        }

        Client.instance.number = (int)objects[1];

        //TODO: findout what this code does, probably for spawning objects in scene
        /*
        if (Client.instance.number != null && Client.instance.objects.Count >= Client.instance.number)
        {

            GameObject go = GameObject.FindGameObjectWithTag("LoadingScreen");

            if (go == null) return;

            go.SetActive(false);

        }
        */

    }

    public override void Write() { }

}
