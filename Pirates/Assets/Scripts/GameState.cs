using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameState : MonoBehaviour
{
    [SerializeField]
    AudioClip gameLost;

    [SerializeField]
    AudioClip gameWon;

    [SerializeField]
    AudioClip death;

    private AudioSource audio;

    public enum State
    {
        NoState,
        WaitForConnections,
        GameRunning,
        Win,
        Lose,
        Death
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
    private GameObject localPlayer;

    private GameObject backgrounds;
    private GameObject backgroundInitial;
    private GameObject backgroundWin;
    private GameObject backgroundLose;
    private GameObject backgroundDeath;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        oldState = State.NoState;
        state = State.WaitForConnections;

        startGameButton = GameObject.Find("StartGame");
        gameGui = GameObject.Find("GameGUI");
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        cowsBullsManager = GameObject.FindGameObjectWithTag("CowsBullsManager");
        timeManager = GameObject.Find("Timer Manager");

        backgrounds = GameObject.Find("Backgrounds");
        backgroundInitial = GameObject.Find("Background_initial");
        backgroundWin = GameObject.Find("Background_win");
        backgroundLose = GameObject.Find("Background_lose");
        backgroundDeath = GameObject.Find("Background_death");

        GoToState(State.WaitForConnections);
	}

    public GameObject GetCowsBullsManager()
    {
        return cowsBullsManager;
    }

    public void SetLocalPlayer(GameObject player)
    {
        localPlayer = player;
    }
    
    public void GoToState(State _state)
    {
        state = _state;
        CheckStateChange();
    }

    public void OnStartGame()
    {
        localPlayer.GetComponent<PlayerServerCommunication>().startGame = true;
    }
	
	void Update()
    {
        CheckStateChange();

        if( state <= State.GameRunning && 
            localPlayer && 
            localPlayer.GetComponent<PlayerServerCommunication>().startGame )
        {
            GoToState(State.GameRunning);
        }

        // cheats //////////////////////////////////
        if (Input.GetKeyDown(KeyCode.U))
        {
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
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
                    startGameButton.SetActive(true);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);

                    backgrounds.SetActive(false);
                    backgroundInitial.SetActive(true);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(false);
                    backgroundDeath.SetActive(false);
                    break;

            case State.GameRunning:
                    startGameButton.SetActive(false);
                    networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;
                    gameGui.SetActive(true);
                    cowsBullsManager.SetActive(true);
                    timeManager.SetActive(true);
                    
                    backgrounds.SetActive(true);
                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(false);
                    backgroundDeath.SetActive(false);
                    break;

            case State.Win:
                    startGameButton.SetActive(false);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                    
                    backgrounds.SetActive(true);
                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(true);
                    backgroundLose.SetActive(false);
                    backgroundDeath.SetActive(false);
                    audio.PlayOneShot(gameWon);
                    break;

            case State.Lose:
                    startGameButton.SetActive(false);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                    
                    backgrounds.SetActive(true);
                    for (int i = 0; i < backgrounds.transform.childCount; i++)
                    {
                        backgrounds.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(64, 64, 64);
                    }

                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(true);
                    backgroundDeath.SetActive(false);
                    audio.PlayOneShot(gameLost);
                break;
            case State.Death:
                    startGameButton.SetActive(false);
                    gameGui.SetActive(false);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);

                    backgrounds.SetActive(true);
                    for (int i = 0; i < backgrounds.transform.childCount; i++)
                    {
                        backgrounds.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(64, 64, 64);
                    }

                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(false);
                    backgroundDeath.SetActive(true);
                    audio.PlayOneShot(death);
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
