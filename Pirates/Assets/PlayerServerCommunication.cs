using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

public class PlayerServerCommunication : NetworkBehaviour {

    NetworkClient myClient;

    public class MessageTypes
    {
        public static short PuzzleType = 1000;
    };
    
    PlayerServerCommunication()
    {
        SetupClient();
    }

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
        Puzzle puzzle = new Puzzle();
        puzzle.puzzleInt = 222;
        NetworkServer.SendToAll(MessageTypes.PuzzleType, puzzle);
    }

    public class Puzzle : MessageBase
    {
        public int puzzleInt;
    }

    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MessageTypes.PuzzleType, ClientSideFunction);
        print(LocalIPAddress());
        myClient.Connect(LocalIPAddress(), 7777);
    }

    public string LocalIPAddress()
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
        Puzzle msg = netMsg.ReadMessage<Puzzle>();
        Debug.Log("Puzzle is " + msg.puzzleInt);
    }

}
