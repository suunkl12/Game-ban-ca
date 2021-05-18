using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
public class NetworkManager : MonoBehaviour
{
    public static string ip = "127.0.0.1";
    public static short port = 26000;
    public static NetworkManager instance;
    private static TcpClient tcp;

    public bool connected = false;
    public Thread threadReceive;
    public Thread threadAttempting;

    public IPEndPoint IpTCP;
    // Start is called before the first frame update
    void Start()
    {
        //instance = this;
        //IPAddress mIp = IPAddress.Parse(ip);
        //IpTCP = new IPEndPoint(mIp, 26000);
        //instance = this;
        //tcp = new TcpClient();
        //tcp.NoDelay = true;
        //Debug.Log("Connecting...");
        //tcp.Connect(IpTCP);
        //Debug.Log("Connected");

    }

    

    

    

}
