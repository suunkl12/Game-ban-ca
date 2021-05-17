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
        gameEvent.OnDisconnect += EnableConnectButtonCli;
        gameEvent.OnConnect += DisableConnectButton;
    }
    [ClientCallback]
    public void EnableConnectButtonCli(NetworkConnection networkConnection)
    {
        connectButton.gameObject.SetActive(true);
    }

    [ClientCallback]
    public void DisableConnectButton(NetworkConnection networkConnection)
    {
        connectButton.gameObject.SetActive(false);
    }
}
