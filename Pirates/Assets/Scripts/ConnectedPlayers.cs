﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectedPlayers : NetworkBehaviour
{
    public Dictionary<string, GameObject> Players;
    public int ConnectedPlayersCount = 0;

    public Dictionary<string, GameObject> TeamA;
    public int TeamAConnectedPlayersCount = 0;

    public Dictionary<string, GameObject> TeamB;
    public int TeamBConnectedPlayersCount = 0;
    
    void Start()
    {
        Players = new Dictionary<string, GameObject>();
        TeamA = new Dictionary<string, GameObject>();
        TeamB = new Dictionary<string, GameObject>();
	}
	
    private bool gameStarted;

	void Update()
    {
        if( !gameStarted ) // if someone starts the game, everyone starts the game
        {
            foreach( KeyValuePair<string, GameObject> kvp in Players )
            {
                if( kvp.Value.GetComponent<PlayerServerCommunication>().startGame )
                {
                    gameStarted = true;
                    break;
                }
            }

            if( gameStarted )
            {
                foreach( KeyValuePair<string, GameObject> kvp in Players )
                {
                    kvp.Value.GetComponent<PlayerServerCommunication>().startGame = true;
                }
            }
        }
	}

    public string AddPlayer(GameObject player)
    {
        string id = player.GetComponent<NetworkIdentity>().netId.ToString();

        if( id == "0" ) // when we use cheats we fake it
        {
            id = (ConnectedPlayersCount + 1).ToString();
        }

        Players.Add(id, player);

        if( ConnectedPlayersCount % 2 == 0 )
        {
            TeamA.Add(id, player);
            ++TeamAConnectedPlayersCount;
        }
        else
        {
            TeamB.Add(id, player);
            ++TeamBConnectedPlayersCount;
        }

        ++ConnectedPlayersCount;

        return id;
    }

    public void RemovePlayer(string id)
    {
        Players.Remove(id);

        if(TeamA.ContainsKey(id))
        {
            TeamA.Remove(id);
            --TeamAConnectedPlayersCount;
        }
        else
        {
            TeamB.Remove(id);
            --TeamBConnectedPlayersCount;
        }

        --ConnectedPlayersCount;
    }

    public GameObject GetPlayer(string id)
    {
        try
        {
            return Players[id];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    public bool PlayerIsInTeamA(string id)
    {
        return TeamA.ContainsKey(id);
    }

    public Dictionary<string, GameObject> GetPlayersFromTeam(bool teamA)
    {
        return teamA ? TeamA : TeamB;
    }

    public Dictionary<string, GameObject> GetPlayersFromTeamA()
    {
        return TeamA;
    }

    public Dictionary<string, GameObject> GetPlayersFromTeamB()
    {
        return TeamB;
    }

    public List<GameObject> GetOrderedPlayers()
    {
        List<GameObject> someOrder = new List<GameObject>();

        foreach( KeyValuePair<string, GameObject> kvp in Players )
        {
            someOrder.Add(kvp.Value);
        }

        return someOrder;
    }

    public List<GameObject> GetOrderedPlayersFromTeamA()
    {
        List<GameObject> someOrder = new List<GameObject>();

        foreach( KeyValuePair<string, GameObject> kvp in TeamA )
        {
            someOrder.Add(kvp.Value);
        }

        return someOrder;
    }

    public List<GameObject> GetOrderedPlayersFromTeamB()
    {
        List<GameObject> someOrder = new List<GameObject>();

        foreach( KeyValuePair<string, GameObject> kvp in TeamB )
        {
            someOrder.Add(kvp.Value);
        }

        return someOrder;
    }
}
