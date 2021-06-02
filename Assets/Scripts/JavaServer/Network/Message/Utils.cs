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
        Add(Tags.INFO, typeof(ClientInfoPacket));
        Add(Tags.PLAYER_SPAWN, typeof(PlayerSpawnPacket));
        Add(Tags.OBJECT_DESPAWN, typeof(ObjectDespawnPacket));
        Add(Tags.PLAYER_SHOT, typeof(PlayerShootPacket));
        Add(Tags.OBJECT_SPAWN, typeof(ObjectSpawnPacket));
        Add(Tags.OBJECT_MOVE, typeof(ObjectMovePacket));
    }


    //TODO: debug to findout what this does
    public static MessagePacket getPacket(Packet p)
    {
        if (!types.ContainsKey(p.Id)) return null;

        MessagePacket msg = (MessagePacket)Activator.CreateInstance(types[p.Id]);
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