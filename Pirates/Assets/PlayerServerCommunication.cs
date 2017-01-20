using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerServerCommunication : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public class Puzzle : MessageBase
    {
        public int puzzleInt;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.P))
        {
            //SolvePuzzle();
        }
	}

    


}
