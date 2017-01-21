using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameState : MonoBehaviour
{
    public enum State
    {
        NoState,
        WaitForConnections,
        GameRunning,
        Win,
        Lose
    }

    private State oldState;
    public State state;
    
    private GameObject canvas;
    private GameObject networkManager;
    private GameObject cowsBullsManager;
    private GameObject timeManager;

	void Start()
    {
        oldState = State.NoState;
        state = State.WaitForConnections;

        canvas = GameObject.Find("Canvas");
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        cowsBullsManager = GameObject.FindGameObjectWithTag("CowsBullsManager");
        timeManager = GameObject.Find("Timer Manager");
	}

    public void GoToState(State _state)
    {
        state = _state;
        CheckStateChange();
    }
	
	void Update()
    {
        CheckStateChange();
	}

    private void CheckStateChange()
    {
        if( state != oldState )
        {
            switch( state )
            {
            case State.NoState:
                {
                    // do nothing
                }
                break;

            case State.WaitForConnections:
                {
                    canvas.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                }
                break;

            case State.GameRunning:
                {
                    networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;
                    canvas.SetActive(true);
                    cowsBullsManager.SetActive(true);
                    timeManager.SetActive(true);
                }
                break;

            case State.Win:
                {
                    canvas.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                }
                break;

            case State.Lose:
                {
                    canvas.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                }
                break;
            }

            oldState = state;
        }
    }
}
