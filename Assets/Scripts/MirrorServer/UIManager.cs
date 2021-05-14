using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameEvent gameEvent;
    public Button connectButton;
    private void Start()
    {
        gameEvent.OnDisconnect += EnableConnectButton;
        gameEvent.OnConnect += DisableConnectButton;
    }
    public void EnableConnectButton(NetworkConnection networkConnection)
    {
        connectButton.gameObject.SetActive(true);
    }
    public void DisableConnectButton(NetworkConnection networkConnection)
    {
        connectButton.gameObject.SetActive(false);
    }
}
