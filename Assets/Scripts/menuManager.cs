using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    public GameObject m_PannelSettings, m_PannelSeeMore, m_ButtonMore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Settings()
	{
        m_PannelSettings.GetComponent<Animator>().SetTrigger("Pop");
	}

    public void SeeMore()
	{
        m_PannelSeeMore.GetComponent<Animator>().SetTrigger("PopMore");
        m_ButtonMore.GetComponent<Animator>().SetTrigger("turn");

    }
}
