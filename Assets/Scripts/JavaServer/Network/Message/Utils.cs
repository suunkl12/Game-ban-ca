using System;
using System.Collections.Generic;
using System.Linq;

public class Utils
{

    public static Dictionary<int, Type> types = new Dictionary<int, Type>();
    public static Dictionary<Type, int> ids = new Dictionary<Type, int>();

    static Utils()
    {
        Add(Tags.PING, typeof(PingPacket));

    }

    public static Message getPacket(Packet p)
    {
        if (!types.ContainsKey(p.Id)) return null;

        Message msg = (Message)Activator.CreateInstance(types[p.Id]);
        msg.Initialize(p.Msg);

        return msg;

    }

    public static int? GetID(Type t)
    {

        if (!ids.ContainsKey(t)) return null;
        else return ids[t];

    }

    public static void Add(int id, Type t)
    {

        types.Add(id, t);
        ids.Add(t, id);

    }

}