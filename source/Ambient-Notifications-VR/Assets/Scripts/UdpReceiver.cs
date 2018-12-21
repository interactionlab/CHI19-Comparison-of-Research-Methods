using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System;

public class UdpReceiver : MonoBehaviour {

    public ChangeColor Plant;
    public ChangeColor Cup;
    public ChangeColor Speaker;
    public ChangeColor Peppermill;
    public ChangeColor Saltmill;


    //source: https://stackoverflow.com/questions/37131742/how-to-use-udp-with-unity-methods
    private static readonly object lockObject = new object();
    private string receivedCommand = "";
    private bool commandReceived = false;
    private Thread udpThread;
    private bool running = true;
    private UdpClient udpClient;


    // Use this for initialization
    void Start () {
        udpClient = new UdpClient(12000);
        udpThread = new Thread(new ThreadStart(receiveUdp));
        udpThread.Start();
        if(Plant == null)
        {
            Debug.LogError("Plant GameObject is not set");
        }
        if(Cup == null)
        {
            Debug.LogError("Cup GameObject is not set");
        }
        if(Speaker == null)
        {
            Debug.LogError("Speaker GameObject is not set");
        }
        if(Peppermill == null)
        {
            Debug.LogError("Peppermill GameObject is not set");
        }
        if (Saltmill == null)
        {
            Debug.LogError("Saltmill GameObject is not set");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (commandReceived)
        {
            lock (lockObject)
            {
                commandReceived = false;
                Dictionary<string, string> commandValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(receivedCommand);
                if (commandValues.ContainsKey("name") && commandValues.ContainsKey("value"))
                {
                    int newVal = Int32.Parse(commandValues["value"]);
                    switch (commandValues["name"])
                    {
                        case ("cup"):
                            if (Cup != null)
                            {
                                Cup.setColor(newVal);
                            }
                            break;
                        case ("peppermill"):
                            if (Peppermill != null)
                            {
                                Peppermill.setColor(newVal);
                            }
                            break;
                        case ("saltmill"):
                            if (Saltmill != null)
                            {
                                Saltmill.setColor(newVal);
                            }
                            break;
                        case ("plant"):
                            if (Plant != null)
                            {
                                Plant.setColor(newVal);
                            }
                            break;
                        case ("speaker"):
                            if (Speaker != null)
                            {
                                Speaker.setColor(newVal);
                            }
                            break;
                    }
                }
            }
        }
	}

    void OnApplicationQuit()
    {
        running = false;
        udpClient.Close();
        udpThread.Abort();
    }

    void receiveUdp()
    {
        while (running)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);
            Debug.Log(returnData);
            /*lock object to make sure there data is 
            *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                receivedCommand = returnData;
                Debug.Log(receivedCommand);
                commandReceived = true;
            }
        }
    }

}
