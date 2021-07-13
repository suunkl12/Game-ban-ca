using System.Reflection;
using UnityEngine;
using System;
public class ObjectMovePacket : MessagePacket
{
    static ObjectMovePacket(){
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType,
            new System.Type[] {
                typeof(int), //id
                typeof(ObjectType), // object type : bullet | fish
                typeof(float), // position x
                typeof(float), // position y
                typeof(float) // rotation
            });
    }

    public ObjectMovePacket(){}

    public override void Read()
    {
        int id = (int)objects[0];
        ObjectType ot = (ObjectType)objects[1];
        switch (ot)
        {
            case ObjectType.BULLET:
                if (!Client.instance.bullets.ContainsKey(id)) return;
                Client.instance.bullets[id].gameObject.transform.position = new Vector3((float)objects[2], (float)objects[3] ,0);
                break;
            case ObjectType.FISH:
                if (!Client.instance.fishes.ContainsKey(id)) return;
                Client.instance.fishes[id].gameObject.transform.position = new Vector3((float)objects[2], (float)objects[3], 0);
                break;
        }
    }

    public override void Write()
    {
        throw new System.NotImplementedException();
    }
}
