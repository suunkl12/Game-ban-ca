using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class changeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable m_firstInput;
    public Button m_buttonLogin;
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        m_firstInput.Select();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftShift))
		{
            Selectable m_previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
			if (m_previous != null)
			{
                m_previous.Select();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
            Selectable m_next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			if (m_next != null)
			{
                m_next.Select();
			}
        }
        else if (Input.GetKeyDown(KeyCode.Return))
		{
            m_buttonLogin.onClick.Invoke();
            Debug.Log("Login Success!");
		}

    }
}
