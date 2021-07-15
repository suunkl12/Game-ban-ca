using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using ProtoBuf;
using Google.Protobuf;

public class Client : MonoBehaviour
{

    public static Client instance;
    private static TcpClient tcp;
    public static string ip = "127.0.0.1";
    public static short port = 4296;
    public IPEndPoint IpTCP;
    public long when;


    public int? id = null;
    public int? number = null;

    public Dictionary<int, GunController> guns = new Dictionary<int, GunController>();
    public Dictionary<int, GameObject> bullets = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> fishes = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    [Header("Prefabs")]
    //Player ở đây là cây súng luôn
    public GameObject player;
    public GameObject bullet;

    public List<GameObject> prefab;

    public bool connected = false;
    public Thread threadReceive;
    public Thread threadAttempting;

    void Start()
    {
        #if UNITY_ANDROID
                //Screen.SetResolution(1920 , 1080,true);
        #endif
        //Khởi tạo một object với component Unity thread
        //true là object hiện hình, false là giấu đi object
        UnityThread.initUnityThread(false);

        foreach (DestructibleType dt in Enum.GetValues(typeof(DestructibleType)).Cast<DestructibleType>())
        {
            prefabs.Add(dt.ToString(), Resources.Load<GameObject>("Objects/" + dt.ToString()));

        }

        foreach (Item i in Enum.GetValues(typeof(Item)).Cast<Item>())
        {

            prefabs.Add(i.ToString(), Resources.Load<GameObject>("Items/" + i.ToString()));

        }

        instance = this;
        tcp = new TcpClient();
        tcp.NoDelay = true;

        Connect("127.0.0.1",4296);
    }

    public void ButtonConnect()
    {
        Connect(ip, port);
    }

    public void Connect(string ip, int porttcp)
    {
        IPAddress mIp = IPAddress.Parse(ip);
        IpTCP = new IPEndPoint(mIp, porttcp);

        threadAttempting = new Thread(new ThreadStart(Attempting))
        {
            IsBackground = true
        };

        threadAttempting.Start();

        Debug.Log("Connecting...");

    }


    //TODO: sửa lại thành true và hiển thị màn hình disconnect
    public void Disconnect() { Disconnect(true); }

    public void Disconnect(bool show)
    {

        try
        {
            if (connected)
            {
                connected = false;
                Debug.Log("Disconnected");

                if (show)
                    UnityThread.executeInUpdate(() =>
                    {

                        foreach (Transform t in GameObject.FindGameObjectWithTag("Canvas").transform)
                        {

                            if (t.name == "DisconnectScreen")
                            {
                                t.gameObject.SetActive(true);
                            }

                        }

                    });

                tcp.GetStream().Close();
                tcp.Close();
                threadReceive.Abort();
            }
        }
        catch (Exception ex) { }

    }

    private void Attempting()
    {

        while (true)
        {
            if (connected) { threadAttempting.Interrupt(); break; }

            TryConnect();

            Thread.Sleep(250);
        }

    }

    
    private bool TryConnect()
    {

        try
        {
            tcp.Connect(IpTCP);

            // Nếu như connect là false thì không attemp connect nữa
            connected = true;

            Debug.Log("Connected");
            
            // Bắt đầu nhận thông tin từ phía server
            threadReceive = new Thread(new ThreadStart(Receive))
            {
                //Background thread không ngăn chương trình dừng lại, chương trình dừng thì background thread sẽ dừng luôn
                IsBackground = true
            };

            threadReceive.Start();


            //Chưa cần phải hiện ping
            //UnityThread.executeInUpdate(() => StartCoroutine( Pinger() ));

            return true;
        }
        catch (Exception ex)
        {

            return false;

        }

    }

    public void Send()
    {
        if (!connected) return;
        try
        {

            NetworkStream stream = tcp.GetStream();
            if (stream.CanWrite)
            {

                var p = new Packet() { Id = 56, Msg = "This is a test message" };

                //Protobuf-net nhanh gọn lẹ, nhưng chỉ xài được chung với C# với nhau ( tức là server và client là c#), còn nếu server là Java,
                //xài Protobuf của google để đảm bảo tính compability

#region Protobuf-net error
                //byte[] b;
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    //Serializer.PrepareSerializer<Package>(); 
                //    Serializer.Serialize(memoryStream, p);
                //    b = memoryStream.ToArray();


                //    byte[] a = new byte[b.Length +1];
                //    a[0] = b[0];
                //    for(int i=0; i < b.Length; i++)
                //    {
                //        a[i+1] = b[i];
                //    }
                //    stream.WriteAsync(a, 0, a.Length);

                //}
#endregion


                MemoryStream rawOutput = new MemoryStream();
                CodedOutputStream output = new CodedOutputStream(rawOutput);
                output.WriteMessage(p);
                output.Flush();
                var b = rawOutput.ToArray();


                stream.WriteAsync(b, 0, b.Length);

                //byte[] b = ProtobufEncoding.Encode(new Packet() { Id = (int)id, Msg = p.ToString() }, false);

            }


        }
        catch (InvalidOperationException ex)
        {
            Debug.LogError(ex.Message);
            Disconnect();
        }
        catch (SocketException ex)
        {
            Debug.LogError(ex.Message);
            Disconnect();
        }

    }


    
    private IEnumerator Pinger()
    {

        while (true)
        {
            if (!connected) break;

            new PingPacket(1).Write();

            yield return new WaitForSeconds(0.8f);
        }

    }
    


    private void Receive()
    {
        if (!connected) return;

        Byte[] bytes = new Byte[1024];
        int lengthl = 0;
        while (true)
        {
            try
            {
                if (!connected) return;
                NetworkStream stream = tcp.GetStream();

                int length;

                //Read trả về length của byte array, có lẽ cái này dùng để check length trước khi làm bất cứ thứ gì khác
                
                while ((length = stream.Read(bytes, lengthl, bytes.Length - lengthl)) > 0)
                {
                    Debug.Log("A message has been recieve");
                    //Sau khi check xong thì biến bytes đã có thông tin, nên ta decode nó
                    KeyValuePair<byte[], List<Google.Protobuf.IMessage>> pair = ProtobufEncoding.Decode(bytes);

                    if (pair.Key.Length > 0)
                    {
                        Array.Copy(pair.Key, 0, bytes, 0, pair.Key.Length);
                        lengthl = pair.Key.Length;
                        Array.Clear(bytes, lengthl, bytes.Length - lengthl);
                    }
                    else
                    {
                        Array.Clear(bytes, 0, bytes.Length);
                        lengthl = 0;
                    }

                    foreach (Packet p in pair.Value.Cast<Packet>())
                    {
                        MessagePacket msg = Utils.getPacket(p);

                        if (msg != null)
                        {
                            msg.ReadSync();
                        }

                    }

                }
            }
            catch (IOException ex)
            {
                Disconnect();
            }
            catch (SocketException ex)
            {
                Disconnect();
            }
            catch (InvalidOperationException ex)
            {
                Disconnect();
            }
        }
    }

    public void Send(MessagePacket p)
    {
        if (!connected) return;

        try
        {

            NetworkStream stream = tcp.GetStream();
            if (stream.CanWrite)
            {

                int? id = Utils.GetID(p.GetType());
                if (id == null) return;

                byte[] b = ProtobufEncoding.Encode(new Packet() { Id = (int)id, Msg = p.ToString() }, false);

                stream.WriteAsync(b, 0, b.Length);

            }


        }
        catch (InvalidOperationException ex)
        {
            Disconnect();
        }
        catch (SocketException ex)
        {
            Disconnect();
        }

    }

    

    public void OnApplicationQuit()
    {

        Disconnect(false);

    }

}