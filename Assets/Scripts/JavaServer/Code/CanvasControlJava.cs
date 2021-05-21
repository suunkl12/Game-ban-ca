using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasControlJava : MonoBehaviour
{
    //// Start is called before the first frame update
    //public Game_Manager game_Manager;
    //public Toggle ChangeSlot;
    //public GameObject urhContainer;
    //public GameObject btnContainer;
    //public Text ScoreTxt;
    //public Text bulletAmountTxt;
    //public Gun[] slotGun;
    //public GameObject[] urh;
    //public Button[] btn;
    //void Start()
    //{
    //    ChangeSlot.isOn = false;
    //    btn = new Button[btnContainer.transform.childCount];
    //    for (int i = 0; i < btnContainer.transform.childCount; i++)
    //    {
    //        btn[i] = btnContainer.transform.GetChild(i).gameObject.GetComponent<Button>();
    //    }
    //    urh = new GameObject[urhContainer.transform.childCount];
    //    for (int i = 0; i < urhContainer.transform.childCount; i++)
    //    {
    //        urh[i] = urhContainer.transform.GetChild(i).gameObject;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    slotGun = game_Manager.slotGun;
    //    for (int i = 0; i < slotGun.Length; i++)
    //    {
    //        if (slotGun[i].inUsed)
    //        {
    //            btn[i].interactable = false;
    //        }
    //        else
    //            btn[i].interactable = true;
    //    }
    //    if (ChangeSlot.isOn)
    //    {
    //        btnContainer.gameObject.SetActive(true);
    //    }
    //    else
    //        btnContainer.gameObject.SetActive(false);

    //    Display();
    //}

    //public void switchGun(int i)
    //{
    //    if(slotGun[i].inUsed == false)
    //    {
    //        game_Manager.playerIn.inUsed = false;
    //        slotGun[i].inUsed = true;
    //        game_Manager.playerIn = slotGun[i];
    //        //Debug.Log("Switching to " + slotGun[i]);
    //    }
    //}


    //public void Display()
    //{

    //    //Nếu như không phải người chơi trên máy,không hiện đạn
    //    if (!isLocalPlayer)
    //        return;
    //    else
    //    {
    //        //Hiển thị số lượng đạn
    //        bulletAmountTxt.text = game_Manager.playerIn.BulletAmount.ToString();
    //        //Hiển thị số điểm
    //        //ScoreTxt.text = game_Manager.playerIn.Score.ToString();

    //        //Hiển thị vị trí người chơi

    //        for (int i = 0; i < slotGun.Length; i++)
    //        {
    //            if (slotGun[i] == game_Manager.playerIn)
    //            {
    //                urh[i].SetActive(true);
    //            }
    //            else
    //                urh[i].SetActive(false);
    //        }
    //    }


    //}    


}
