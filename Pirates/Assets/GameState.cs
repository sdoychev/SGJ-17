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
    
    private GameObject startGameButton;
    private GameObject gameGui;
    private GameObject networkManager;
    private GameObject cowsBullsManager;
    private GameObject timeManager;

	void Start()
    {
        oldState = State.NoState;
        state = State.WaitForConnections;

        startGameButton = GameObject.Find("StartGame");
        gameGui = GameObject.Find("GameGUI");
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        cowsBullsManager = GameObject.FindGameObjectWithTag("CowsBullsManager");
        timeManager = GameObject.Find("Timer Manager");

        GoToState(State.WaitForConnections);
	}

    public GameObject GetCowsBullsManager()
    {
        return cowsBullsManager;
    }

    public void GoToState(State _state)
    {
        state = _state;
        CheckStateChange();
    }

    public void OnStartGame()
    {
        GoToState(State.GameRunning);
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
                    startGameButton.SetActive(true);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                }
                break;

            case State.GameRunning:
                {
                    startGameButton.SetActive(false);
                    networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;
                    gameGui.SetActive(true);
                    cowsBullsManager.SetActive(true);
                    timeManager.SetActive(true);
                }
                break;

            case State.Win:
                {
                    startGameButton.SetActive(false);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                }
                break;

            case State.Lose:
                {
                    startGameButton.SetActive(false);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                }
                break;
            }

            oldState = state;
        }
    }
}
