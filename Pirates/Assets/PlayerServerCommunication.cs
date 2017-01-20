using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

public class PlayerServerCommunication : NetworkBehaviour
{
    public class MessageTypes
    {
        public static short PuzzleType = 1000;
    };

    public class PuzzleMessage : MessageBase
    {
        public int puzzleInt;
    }

    NetworkClient mLocalClient;

    
    /*
    PlayerServerCommunication()
    {
        SetupClient();
    }*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CmdSolvePuzzle();
        }
    }

    [Command]
    void CmdSolvePuzzle()
    {
        PuzzleMessage puzzleMessage = new PuzzleMessage();
        puzzleMessage.puzzleInt = 222;
        NetworkServer.SendToAll(MessageTypes.PuzzleType, puzzleMessage);
    }
    
    public void SetupClient()
    {
        mLocalClient = new NetworkClient();

        mLocalClient.RegisterHandler(MessageTypes.PuzzleType, ClientSideFunction);

        print(GetLocalIPAddress());
        mLocalClient.Connect(GetLocalIPAddress(), 7777);
    }

    public string GetLocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    public void ClientSideFunction(NetworkMessage netMsg)
    {
        PuzzleMessage msg = netMsg.ReadMessage<PuzzleMessage>();
        Debug.Log("Puzzle is " + msg.puzzleInt);
    }

}
