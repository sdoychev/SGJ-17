using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBoard : MonoBehaviour
{
    private List<List<GameObject>> MarkersTeamA;
    private List<List<GameObject>> MarkersTeamB;

	void Start()
    {
        MarkersTeamA = new List<List<GameObject>>();

        for( int i = 1; i <= 3; ++i ) // players
        {
            var current = new List<GameObject>();
            
            for( int j = 1; j <= 3; ++j ) // levels
            {
                string objectName = "RoomComplete_A" + i.ToString() + j.ToString();
                GameObject obj = GameObject.Find(objectName);
                if( obj )
                {
                    current.Add( obj );
                    obj.SetActive(false);
                }
            }

            MarkersTeamA.Add(current);
        }

        MarkersTeamB = new List<List<GameObject>>();

        for( int i = 1; i <= 3; ++i ) // players
        {
            var current = new List<GameObject>();
            
            for( int j = 1; j <= 3; ++j ) // levels
            {
                string objectName = "RoomComplete_B" + i.ToString() + j.ToString();
                GameObject obj = GameObject.Find(objectName);
                if( obj )
                {
                    current.Add( obj );
                    obj.SetActive(false);
                }
            }

            MarkersTeamB.Add(current);
        }
	}
	
	void Update()
    {
        GameObject networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        ConnectedPlayers connectedPlayers = networkManager.GetComponent<ConnectedPlayers>();

        List<GameObject> teamA = connectedPlayers.GetOrderedPlayersFromTeamA();
        List<GameObject> teamB = connectedPlayers.GetOrderedPlayersFromTeamB();

        for( int i = 0; i < teamA.Count; ++i ) // players
        {
            int currentLevel = teamA[i].GetComponent<PlayerServerCommunication>().GetCurrentLevel();

            for( int j = 0; j < 3; ++j ) // levels
            {
                GameObject obj = MarkersTeamA[i][j];

                if( obj )
                {
                    obj.SetActive(currentLevel - 1 > j);
                }
            }
        }

        for( int i = 0; i < teamB.Count; ++i ) // players
        {
            int currentLevel = teamB[i].GetComponent<PlayerServerCommunication>().GetCurrentLevel();

            for( int j = 0; j < 3; ++j ) // levels
            {
                GameObject obj = MarkersTeamB[i][j];

                if( obj )
                {
                    obj.SetActive(currentLevel - 1 > j);
                }
            }
        }
	}
}
