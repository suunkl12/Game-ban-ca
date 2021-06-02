using System.Reflection;
using UnityEngine;
using System;
public class ObjectDespawnPacket : MessagePacket
{
    public ObjectDespawnPacket() { }

    static ObjectDespawnPacket()
    {
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType,
            new Type[] {
            typeof(int),
            typeof(ObjectType) });
    }

    public override void Read()
    {
        int id = (int)objects[0];
        ObjectType ot = (ObjectType)objects[1];
        switch (ot)
        {
            case ObjectType.PLAYER:
                //guns đã từng là players
                if (!Client.instance.guns.ContainsKey(id)) return;

                bool was = false;

                //Check thử coi có phải id mình không
                if (Client.instance.id != null && Client.instance.id == id)
                {
                    //Nếu là mình thì hiện death screen lên
                    was = true;

                    foreach (Transform t in GameObject.FindGameObjectWithTag("Canvas").transform)
                    {

                        if (t.name == "DeathScreen")
                        {
                            t.gameObject.SetActive(true);
                        }
                    }
                }
                //Luôn luôn destroy cái object đó, không cần biết có phải mình hay không
                GameObject.Destroy(Client.instance.guns[id].gameObject);

                //Nếu không phải là mình thì remove người chơi đó ra khỏi danh sách
                if (!was) Client.instance.guns.Remove(id);
                break;


            case ObjectType.BULLET:
                if (!Client.instance.bullets.ContainsKey(id)) return;


                GameObject.Destroy(Client.instance.bullets[id].gameObject);
                Client.instance.bullets.Remove(id);
                break;
            case ObjectType.NONE:
                if (!Client.instance.objects.ContainsKey(id)) return;

                GameObject.Destroy(Client.instance.objects[id]);
                Client.instance.objects.Remove(id);
                break;
        }

    }

    public override void Write()
    {
        throw new System.NotImplementedException();
    }
}
