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
        Tutorial,
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
    private GameObject tutorialOverlay;

    private List<GameObject> playerMakersA;
    private List<GameObject> playerMakersB;

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
        tutorialOverlay = GameObject.Find("Tutorial_overlay");

        playerMakersA = new List<GameObject>();
        playerMakersB = new List<GameObject>();

        playerMakersA.Add(GameObject.Find("PlayerMarker_A1"));
        playerMakersA.Add(GameObject.Find("PlayerMarker_A2"));
        playerMakersA.Add(GameObject.Find("PlayerMarker_A3"));
        playerMakersB.Add(GameObject.Find("PlayerMarker_B1"));
        playerMakersB.Add(GameObject.Find("PlayerMarker_B2"));
        playerMakersB.Add(GameObject.Find("PlayerMarker_B3"));

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
        if( state < State.Tutorial )
        {
            GoToState(State.Tutorial);
        }
        else if( state < State.GameRunning )
        {
            localPlayer.GetComponent<PlayerServerCommunication>().startGame = true;
        }
    }
	
	void Update()
    {
        CheckStateChange();

        if( state <= State.GameRunning && 
            localPlayer && 
            localPlayer.GetComponent<PlayerServerCommunication>().startGame )
        {
            GoToState(State.GameRunning);
            ShowPlayerMarker();
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
                    tutorialOverlay.SetActive(false);

                    foreach( GameObject p in playerMakersA ) p.SetActive(false);
                    foreach( GameObject p in playerMakersB ) p.SetActive(false);
                    break;

            case State.Tutorial:
                    startGameButton.SetActive(true);
                    networkManager.GetComponent<NetworkManagerHUD>().showGUI = false;
                    gameGui.SetActive(true);
                    cowsBullsManager.SetActive(false);
                    timeManager.SetActive(false);
                    
                    backgrounds.SetActive(true);
                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(false);
                    backgroundDeath.SetActive(false);
                    tutorialOverlay.SetActive(true);
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
                    tutorialOverlay.SetActive(false);
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
                    tutorialOverlay.SetActive(false);
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
                        backgrounds.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color32(64, 64, 64, 255);
                    }

                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(true);
                    backgroundDeath.SetActive(false);
                    tutorialOverlay.SetActive(false);
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
                        backgrounds.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color32(64, 64, 64, 255);
                    }

                    backgroundInitial.SetActive(false);
                    backgroundWin.SetActive(false);
                    backgroundLose.SetActive(false);
                    backgroundDeath.SetActive(true);
                    tutorialOverlay.SetActive(false);
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

    private void ShowPlayerMarker()
    {
        ConnectedPlayers connectedPlayers = networkManager.GetComponent<ConnectedPlayers>();

        bool isTeamA = localPlayer.GetComponent<PlayerServerCommunication>().isTeamA;
        List<GameObject> team = isTeamA ? connectedPlayers.GetOrderedPlayersFromTeamA() : connectedPlayers.GetOrderedPlayersFromTeamB();
        int playerIndex = 1;
        for( ; playerIndex < 4; ++playerIndex )
        {
            if(team[playerIndex-1] == localPlayer)
            {
                break;
            }
        }
        //string markerId = "PlayerMarker_" + (isTeamA ? "A" : "B") + playerIndex.ToString();

        if( isTeamA )
        {
            playerMakersA[playerIndex - 1].SetActive(true);
        }
        else
        {
            playerMakersB[playerIndex - 1].SetActive(true);
        }
    }
}
