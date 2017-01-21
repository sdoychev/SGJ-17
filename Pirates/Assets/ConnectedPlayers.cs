using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlayers : MonoBehaviour
{
    public List<GameObject> Players;

    public void AddPlayer(GameObject player)
    {
        Players.Add(player);
    }

	void Start ()
    {
	}
	
	void Update ()
    {
	}
}
