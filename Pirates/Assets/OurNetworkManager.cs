using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OurNetworkManager : NetworkManager
{
	void Start ()
    {
	}
	
	void Update ()
    {
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        print("Added player");
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //player.GetComponent<Player>().color = Color.Red;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
