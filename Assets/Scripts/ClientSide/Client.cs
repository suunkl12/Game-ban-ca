using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace CSharpSocket {
    public class Client : MonoBehaviour
    {
        string id = "0";

        public List<Gun> gunList;

        StreamWriter writer;
        NetworkStream stream;

        List<string> clients;

        public GameEvent gameEvent;
        int index;
        // Start is called before the first frame update
        void Start()
        {
            clients = new List<string>();

            

            print("Connection");
            TcpClient client = new TcpClient("localhost", 16000);
            stream = client.GetStream();
            stream.ReadTimeout = 10;
            //stream.WriteTimeout = 2;
            if (stream.CanRead)
            {
                writer = new StreamWriter(stream);
                print("Writer created");
                SendUsedGunIndex();
                ReadData();
            }


        }

        public void SetUsedGunIndex(int index)
        {
            this.index = index;
        }

        public void SendUsedGunIndex()
        {
            Data data = new Data();
            data.index = index;
            data.action = "saveGun";
            string str = JsonUtility.ToJson(data);
            writer.Write(str);
            writer.Flush();
        }

        public void SendShootData(float rotation)
        {
            Data data = new Data(id, "shoot",rotation);
            string str = JsonUtility.ToJson(data);
            writer.Write(str);
            writer.Flush();
        }

        // Update is called once per frame
        void Update()
        {
            ReadData();
        }
        public void ReadData()
        {
            if (stream.CanRead)
            {
                try
                {
                    
                    byte[] bLen = new Byte[4];
                    int data = stream.Read(bLen, 0, 4);
                    if (data > 0)
                    {
                        int len = BitConverter.ToInt32(bLen, 0);
                        //print("len = " + len);
                        Byte[] buff = new byte[1024];
                        try
                        {
                            data = stream.Read(buff, 0, len);
                            if (data > 0)
                            {
                                string result = Encoding.ASCII.GetString(buff, 0, data);
                                Data command = JsonUtility.FromJson<Data>(result);
                                stream.Flush();
                                ParseData(command);
                            }
                        }
                        catch(Exception ex)
                        {
                            Debug.LogError(ex.Message);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void ParseData(Data data)
        {
            switch (data.action)
            {
                case "start":
                    break;
                case "otherGunShoot":
                    gameEvent.OnReceiveRotationOfOtherGunRotateGun(data.index, data.rotation);
                    break;
            }
        }

        public static void Main(string[] args)
        {
            #region ClientTest
            /*
            string toSend = "Hello!";

            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("192.168.100.34"), 4343);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverAddress);
            
            // Sending
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            clientSocket.Send(toSendLenBytes);
            clientSocket.Send(toSendBytes);
            // Receiving
            byte[] rcvLenBytes = new byte[4];
            clientSocket.Receive(rcvLenBytes);
            int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
            byte[] rcvBytes = new byte[rcvLen];
            clientSocket.Receive(rcvBytes);
            string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

            Debug.Log("Client received: " + rcv);

            clientSocket.Close();*/
            #endregion
        }

        public void AddClient(string id)
        {
            clients.Add(id);
        }

        public void UnAssignGun()
        {

        }

        public void OtherPlayerShootGun()
        {

        }

        public void SpawnAnimals()
        {

        }

        public void MoveAnimals()
        {

        }

        public void DamageAnimal()
        {

        }

        public void AnimalDie()
        {

        }



        
    }
}

[Serializable]
public class Data{

    public string id;
    public string action;
    public float rotation;
    public int index;
    public Data()
    {
        rotation = 0;
        action = "none";
    }

    public Data(float rotation, string action)
    {
        this.rotation = rotation;
        this.action = action;
    }

    public Data(string id, string action, float rotation)
    {
        this.id = id;
        this.action = action;
        this.rotation = rotation;
    }

    public Data(string id, string action, float rotation, int index) : this(id, action, rotation)
    {
        this.index = index;
    }
}

