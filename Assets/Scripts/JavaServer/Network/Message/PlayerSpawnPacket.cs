using System;
using System.Reflection;
using UnityEngine;

public class PlayerSpawnPacket : MessagePacket
{

    static PlayerSpawnPacket()
    {
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType,
            new Type[] {
                typeof(int), // ID 0
                typeof(float), // X - Position 1
                typeof(float), // Y - Position 2
                typeof(float), // Z - Quaternion 3
                typeof(float), // W - Quaternion 4
            }
        );
    }

    public PlayerSpawnPacket() { }

    public override void Read()
    {

        
        Debug.Log("Spawning gun");

        // Create a player 
        // parameter 1: player prefab
        // parameter 2: player position
        // parameter 3: player rotation
        //Client.instance.player là prefab của player
        GameObject go = GameObject.Instantiate(Client.instance.player, new Vector2((float)objects[1], (float)objects[2]), Quaternion.Euler(0f, 0f, (float)objects[3]));

        int id = (int)objects[0];

        GunController gc = go.GetComponent<GunController>();
        gc.id = id;
        if(Client.instance.id == gc.id)
        {
            gc.controllable = true;
        }

        Client.instance.guns.Add(id, gc);

        
    }

    public override void Write() { }

}
