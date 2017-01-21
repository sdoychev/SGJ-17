using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

public class PlayerServerCommunication : NetworkBehaviour
{
    public string id;
    public bool isTeamA;

    void Start()
    {
        GameObject networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        ConnectedPlayers connectedPlayers = networkManager.GetComponent<ConnectedPlayers>();
        id = connectedPlayers.AddPlayer(this.gameObject);
        isTeamA = connectedPlayers.PlayerIsInTeamA(id);
	}

    void OnDestroy()
    {
        GameObject networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        networkManager.GetComponent<ConnectedPlayers>().RemovePlayer(id);
    }

    public int oldLevel = 1;
    [SyncVar]
    public int level = 1;

    public bool oldStartGame = false;
    [SyncVar]
    public bool startGame = false;

    void Update()
    {
        if( !isLocalPlayer )
            return;

        if( oldLevel != level )
        {
            CmdLevel(level);
            print("level " + level);
            oldLevel = level;
        }

        if( oldStartGame != startGame )
        {
            CmdStartGame();
            print("Start Game");
            oldStartGame = startGame;
        }
    }

    [Command]
    public void CmdLevel(int l)
    {
        if (!isServer)
            return;

        level = l;
    }

    [Command]
    public void CmdStartGame()
    {
        if (!isServer)
            return;

        startGame = true;
    }

    public override void OnStartLocalPlayer()
    {
        print("OnStartLocalPlayer");

        var gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();

        gameState.SetLocalPlayer(this.gameObject);
        gameState.GetCowsBullsManager().GetComponent<CowsBullsManager>().setLocalPlayer(this.gameObject);
    }

    public void SetCurrentLevel(int _level)
    {
        level = _level;
    }

    public int GetCurrentLevel()
    {
        return level;
    }
}
