using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game_ManagerJava : MonoBehaviour
{
    //public GameObject GunContainer;

    //public Gun[] slotGun;

    //public Gun playerIn;

    //public GameEvent gameEvent;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    gameEvent.SetGunNotInUsed += TakeGunBack;
    //    gameEvent.OnConnect += SetRandom;
    //    slotGun = new Gun[GunContainer.gameObject.transform.childCount];
    //    for (int i = 0; i < GunContainer.gameObject.transform.childCount; i++)
    //    {
    //        slotGun[i] = GunContainer.gameObject.transform.GetChild(i).gameObject.GetComponent<Gun>();
    //    }
    //}

    //void setSlot()
    //{
    //    for (int i = 0; i < slotGun.Length; i++)
    //    {
    //        if (!slotGun[i].inUsed)
    //        {
    //            slotGun[i].inUsed = true;
    //            playerIn = slotGun[i];
                
    //            //Debug.Log("You are in " + slotGun[i]);
    //            break;
    //        }
    //        else
    //            continue;
    //    }
    //}


    //void SetRandom()
    //{
    //    int i;
    //    do
    //    {
    //        //TODO set gun to random
    //        i = Random.Range(0, slotGun.Length - 1);
    //        if (!slotGun[i].inUsed)
    //        {
    //            slotGun[i].SetGunInUsed();
    //            TargetSetPlayerIn(target, i);
    //            //Debug.Log("You are in " + slotGun[i]);
    //            break;
    //        }
            
    //    } while (true);
    //}

    ////Set tình trạng sử dụng của trúng là không có ai xài trên server
    //public void TakeGunBack(int index)
    //{
    //    Debug.Log("Set gun " + index + " not in used");
    //    slotGun[index].ResetGun();
    //}

    ////Set súng này đang được sử dụng bởi người chơi trên client
    //public void TargetSetPlayerIn(NetworkConnection connection, int i)
    //{
    //    playerIn = slotGun[i];
    //}
    
}
