using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using HoloToolkit.Unity;


#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
using Newtonsoft.Json;
#endif

[System.Serializable]
public class UDPMessageEvent : UnityEvent<string, string, byte[]>
{

}

public class UDPCommunication : MonoBehaviour
{
    public ChangeColor Plant;

    public ChangeColor Cup;

    public ChangeColor Speaker;

    public ChangeColor Peppermill;

    public ChangeColor Saltmill;


    [Tooltip ("Port to open on HoloLens to send or listen")]
	public string internalPort = "12000";


	private readonly  Queue<Action> ExecuteOnMainThread = new Queue<Action>();


#if !UNITY_EDITOR

   
	void UDPMessageReceived(string host, string port, byte[] data)
	{
	    //Debug.Log("UDP message from " + host + " on port " + port + ", " + data.Length.ToString() + " bytes ");

    }


	DatagramSocket socket;

	async void Start()
	{

    	Debug.Log("Waiting for a connection...");

	    socket = new DatagramSocket();
	    socket.MessageReceived += Socket_MessageReceived;

	    HostName IP = null;
	    try
	    {
	        var icp = NetworkInformation.GetInternetConnectionProfile();

	        IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
	        .SingleOrDefault(
	        hn =>
	        hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
	        == icp.NetworkAdapter.NetworkAdapterId);

	        await socket.BindEndpointAsync(IP, internalPort);

            Debug.Log("Now listing on " + IP + " Port: " + internalPort);
	    }
	    catch (Exception e)
	    {
	        Debug.Log(e.ToString());
	        Debug.Log(SocketError.GetStatus(e.HResult).ToString());
	        return;
	    }   

	}

#else
    // to make Unity-Editor happy :-)
    void Start()
	{

	}

#endif


    static MemoryStream ToMemoryStream(Stream input)
	{
		try
		{                                         // Read and write in
			byte[] block = new byte[0x1000];       // blocks of 4K.
			MemoryStream ms = new MemoryStream();
			while (true)
			{
				int bytesRead = input.Read(block, 0, block.Length);
				if (bytesRead == 0) return ms;
				ms.Write(block, 0, bytesRead);
			}
		}
		finally { }
	}

	// Update is called once per frame
	void Update()
	{
		while (ExecuteOnMainThread.Count > 0)
		{
           
			ExecuteOnMainThread.Dequeue().Invoke();

		}
	}


#if !UNITY_EDITOR
	    private void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
	    Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
	    {
	        //Debug.Log("GOT MESSAGE FROM: " + args.RemoteAddress.DisplayName);
	        //Read the message that was received from the UDP  client.

	        Stream streamIn = args.GetDataStream().AsStreamForRead();
	        MemoryStream ms = ToMemoryStream(streamIn);
	        byte[] msgData = ms.ToArray();

	        if (ExecuteOnMainThread.Count == 0)
	        {
	            ExecuteOnMainThread.Enqueue(() =>
	            {
	                                
                    ResponseToUDPPacket(args.RemoteAddress.DisplayName, internalPort, msgData);
                });
                
	        }
	}

    public void ResponseToUDPPacket(string fromIP, string fromPort, byte[] data)
    {
        string dataString = System.Text.Encoding.UTF8.GetString(data);

        Dictionary<string, string> commandValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataString);
        if (commandValues.ContainsKey("name") && commandValues.ContainsKey("value"))
        {
            int newVal = Int32.Parse(commandValues["value"]);
            switch (commandValues["name"])
            {
                case ("cup"):
                    if (Cup != null)
                    {
                        Debug.Log("Set color: Cup" + newVal);
                        Cup.setColor(newVal);
                    }
                    break;
                case ("peppermill"):
                    if (Peppermill != null)
                    {
                        Debug.Log("Set color: Peppermill" + newVal);
                        Peppermill.setColor(newVal);
                    }
                    break;
                case ("saltmill"):
                    if (Saltmill != null)
                    {
                        Debug.Log("Set color: Saltmill" + newVal);
                        Saltmill.setColor(newVal);
                    }
                    break;
                case ("plant"):
                    if (Plant != null)
                    {
                        Debug.Log("Set color: Plant" + newVal);
                        Plant.setColor(newVal);
                    }
                    break;
                case ("speaker"):
                    if (Speaker != null)
                    {
                        Debug.Log("Set color: Speaker" + newVal);
                        Speaker.setColor(newVal);
                    }
                    break;
            }
        }

    }

#endif
}
