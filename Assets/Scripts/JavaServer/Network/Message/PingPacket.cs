using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PingPacket : MessagePacket
{

    static PingPacket()
    {
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType, new Type[] {
            typeof(byte)
        });
    }

    public PingPacket() { }

    public PingPacket(byte i)
    {

        objects.Add(i);

    }

    public override void Read()
    {
        Text ping = GameObject.FindGameObjectWithTag("Ping").GetComponent<Text>();
        ping.text = "Delay: " + ((int) (DateTimeOffset.Now.ToUnixTimeMilliseconds() - Client.instance.when)) + " ms";
    }

    public override void Write()
    {
        Client.instance.when = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Client.instance.Send(this);
    }

}