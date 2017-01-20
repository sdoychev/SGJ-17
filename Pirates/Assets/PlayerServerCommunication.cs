using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerServerCommunication : NetworkBehaviour {

    NetworkClient myClient;

    public class MessageTypes
    {
        public static short PuzzleType = 1000;
    };

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
        myClient.Connect("192.168.85.52", 7777);
    }

    public void ClientSideFunction(NetworkMessage netMsg)
    {
        Puzzle msg = netMsg.ReadMessage<Puzzle>();
        Debug.Log("Puzzle is " + msg.puzzleInt);
    }

}
