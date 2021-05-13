using Mirror;
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
        gameEvent.OnDisconnect += ReturnGunBackCmd;
        gameEvent.OnDisconnect += SetGunNotInUsed;
        //Muốn tìm child thì phải thêm têm parent
        if (!isLocalPlayer)
            return;
        CmdOnConnectAssignGun(GetComponent<NetworkIdentity>());
    }

    [TargetRpc]
    public void TargetAssignAuthority(NetworkConnection target, int index)
    {
        currentIndex = index;
        Debug.Log("Gun Container/Gun Pivot (" + (index + 1) + ")");
        gunIdentity = GameObject.Find("Gun Container/Gun Pivot (" + (index + 1) + ")").GetComponent<NetworkIdentity>();
        CmdSetAuthority(gunIdentity);
        
    }

    public void ReturnGunBackCmd()
    {
        CmdRemoveAuthority(gunIdentity);
    }
    
    public void SetGunNotInUsed()
    {
        CmdSetGunNotInUse(currentIndex);
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
        networkIdentity.AssignClientAuthority(connectionToClient);
    }

    [Command]
    void CmdRemoveAuthority(NetworkIdentity networkIdentity)
    {
        Debug.Log("Revoked identity");
        
        networkIdentity.RemoveClientAuthority();
    }

    [Command]
    void CmdSetGunNotInUse(int index)
    {
        gameEvent.SetGunNotInUsed?.Invoke(index);
    }
}
