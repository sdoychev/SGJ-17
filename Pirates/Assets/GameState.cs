﻿using System.Collections;
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

    [SerializeField]
    private GameObject playerPrefab;

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

        // cheats //////////////////////////////////
        if (Input.GetKeyDown(KeyCode.U))
        {
            var newObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        
        if( Input.GetKeyDown(KeyCode.Keypad1) )
        {
            CheatAddLevelForPlayer(0);
        }
        if( Input.GetKeyDown(KeyCode.Keypad2) )
        {
            CheatAddLevelForPlayer(1);
        }
        if( Input.GetKeyDown(KeyCode.Keypad3) )
        {
            CheatAddLevelForPlayer(2);
        }
        if( Input.GetKeyDown(KeyCode.Keypad4) )
        {
            CheatAddLevelForPlayer(3);
        }
        if( Input.GetKeyDown(KeyCode.Keypad5) )
        {
            CheatAddLevelForPlayer(4);
        }
        if( Input.GetKeyDown(KeyCode.Keypad6) )
        {
            CheatAddLevelForPlayer(5);
        }
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

    private void CheatAddLevelForPlayer(int playerIndex)
    {
        var allPlayers = networkManager.GetComponent<ConnectedPlayers>().GetOrderedPlayers();

        var psc = allPlayers[playerIndex].GetComponent<PlayerServerCommunication>();

        psc.SetCurrentLevel(psc.GetCurrentLevel() + 1);
    }
}
