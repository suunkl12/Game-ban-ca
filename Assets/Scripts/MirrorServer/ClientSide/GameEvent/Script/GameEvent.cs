using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
[CreateAssetMenu( fileName ="Listerner",menuName ="ScriptableObject/Listener")]
public class GameEvent : ScriptableObject
{


    public event UnityAction<NetworkConnection> OnConnect = delegate { };

    public void OnConnectAssignGunCmd(NetworkIdentity target)
    {
        //Command
        OnConnect?.Invoke(target.connectionToClient);
    }

    public event UnityAction<NetworkConnection> OnDisconnect = delegate { };
    public UnityAction<int> SetGunNotInUsed;
    public void OnDisconnectTakeGunBackCmd(NetworkConnection clientConnection)
    {
        //Command
        OnDisconnect?.Invoke(clientConnection);
    }

    

    public event UnityAction<NetworkConnection,int> OnAssignGun = delegate { };
    public void OnAssignGunAssignAuthority(NetworkConnection target, int index)
    {
        OnAssignGun?.Invoke(target,index);
    }

    public UnityAction OnShoot;

    public void OnShootSendData()
    {
        OnShoot?.Invoke();
    }

    public UnityEvent<int, float> OnReceiveRotationOfOtherGun = new MyCustomUnityEvent();


    public void OnReceiveRotationOfOtherGunRotateGun(int index, float rotation2d)
    {
        OnReceiveRotationOfOtherGun?.Invoke(index, rotation2d);
    }
}

public class MyCustomUnityEvent : UnityEvent<int, float>
{}

public class MyCustomUnityEvent3 : UnityEvent<float>
{ }