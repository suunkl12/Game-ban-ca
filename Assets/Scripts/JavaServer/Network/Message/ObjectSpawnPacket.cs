
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class ObjectSpawnPacket : MessagePacket
{

    static ObjectSpawnPacket()
    {
        SetTypes(MethodBase.GetCurrentMethod().DeclaringType, new Type[] {
            typeof(int), // ID 0
            typeof(ObjectType), // Type of object 1
            typeof(FistType), // FishType 2
            typeof(float), // X - Position 3
            typeof(float), // Y - Position 4
            typeof(float), // Rotation 5
            typeof(int) // Health for destructible object 6
        });
    }

    public ObjectSpawnPacket() { }

    public override void Read()
    {

        int id = (int)objects[0];


        ObjectType ot = (ObjectType)objects[1];

        if(ot == ObjectType.BULLET)
        {
            if (Client.instance.bullets.ContainsKey(id)) return;
            GameObject bullet = GameObject.Instantiate(Client.instance.bullet, new Vector2((float)objects[3], (float)objects[4]), Quaternion.Euler(0,0,(float)objects[5]));
            Client.instance.bullets.Add(id, bullet);
            return;
        }

        if(ot == ObjectType.FISH)
        {
            if (Client.instance.animals.ContainsKey(id)) return;
            FistType animalType = (FistType)objects[2];

            GameObject animal = GameObject.Instantiate(Client.instance.prefab[(int)animalType], new Vector2((float)objects[3], (float)objects[4]), Quaternion.Euler(0, 0, (float)objects[5]));
            animal.GetComponent<AnimalHealth>().Id = id;
            Client.instance.animals.Add(id, animal);


            return;
        }

        //FistType type = (FistType)objects[2];

        //if (!Client.instance.prefabs.ContainsKey(type)) return;
        //GameObject prefab = Client.instance.prefabs[type];

        //GameObject go = GameObject.Instantiate(prefab, new Vector2((float)objects[3], (float)objects[4]), Quaternion.identity);
        //Client.instance.objects.Add(id, go);


        if (Client.instance.number != null && Client.instance.objects.Count >= Client.instance.number)
        {

            GameObject go2 = GameObject.FindGameObjectWithTag("LoadingScreen");

            if (go2 == null) return;

            go2.SetActive(false);

        }

    }


    
    public override void Write() { }

}
