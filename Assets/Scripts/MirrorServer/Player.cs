using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public GameEvent gameEvent;

    NetworkIdentity gunIdentity;

    int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        gameEvent.OnAssignGun += TargetAssignAuthority;
        gameEvent.OnAssignGun += AssignGunIndex;
        gameEvent.OnDisconnect += TakeGunAuthorityBack;
        gameEvent.OnDisconnect += SetGunNotInUsed;
        //Muốn tìm child thì phải thêm têm parent
        if (!isLocalPlayer)
            return;
        CmdOnConnectAssignGun(GetComponent<NetworkIdentity>());
    }

    //Set index của gun on server
    private void AssignGunIndex(NetworkConnection target, int index)
    {
        currentIndex = index;
    }


    [TargetRpc]
    public void TargetAssignAuthority(NetworkConnection target, int index)
    {
        currentIndex = index;
        Debug.Log("Gun Container/Gun Pivot (" + (index + 1) + ")");
        // Set indentity của súng trên client
        gunIdentity = GameObject.Find("Gun Container/Gun Pivot (" + (index + 1) + ")").GetComponent<NetworkIdentity>();
        CmdSetAuthority(gunIdentity);
        
    }

    public void TakeGunAuthorityBack(NetworkConnection clientConnection)
    {
        try
        {
            Debug.Log("Revoked identity");
            //NetworkConnection a;
            //a.identity
            if(clientConnection.identity == GetComponent<NetworkIdentity>())
            gunIdentity.RemoveClientAuthority();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    
    public void SetGunNotInUsed(NetworkConnection clientConnection)
    {
        gameEvent.SetGunNotInUsed?.Invoke(currentIndex);
    }

    [Command]
    public void CmdOnConnectAssignGun(NetworkIdentity target)
    {
        gameEvent.OnConnectAssignGunCmd(target);
    }

    [Command]
    void CmdSetAuthority(NetworkIdentity networkIdentity)
    {
        Debug.Log("Assigned identity");

        // Set indentity của súng trên server
        gunIdentity = networkIdentity;

        networkIdentity.AssignClientAuthority(connectionToClient);
    }

    [Command]
    void CmdSetGunNotInUse(int index)
    {
        gameEvent.SetGunNotInUsed?.Invoke(index);
    }
}
