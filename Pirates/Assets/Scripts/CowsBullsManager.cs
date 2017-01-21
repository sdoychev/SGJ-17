using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class CowsBullsManager : MonoBehaviour {

    [SerializeField]
    private int currentLevel = 1;

    [SerializeField]
    private GameObject backgrounds;

    [SerializeField]
    private GameObject wheels;

    [SerializeField]
    private GameObject submitButton;

    [SerializeField]
    private List<int> puzzle;

    [SerializeField]
    private Text attemptsBoard;

    [SerializeField]
    private GameObject TimeManagerObject;

    private GameObject localPlayer;

    private float startTime;
    private bool swapBackground = false;
    private Vector3 newBackgroundPosition;
    private TimerManager timeManager;

    private GameObject[] wheelsForNextLevel;

    private void Start()
    {
        submitButton.GetComponent<Button>().interactable = false;
        GenerateNewPuzzle();
        timeManager = TimeManagerObject.GetComponent<TimerManager>();
    }

    private List<int> GenerateNewPuzzle()
    {
        puzzle = new List<int>();
        int randomRange = 5;
        switch (currentLevel)
        {
            default:
            case 1:
                randomRange = 5;
                break;
            case 2:
                randomRange = 7;
                break;
            case 3:
                randomRange = 9;
                break;
        }
        while (puzzle.Count < 4)
        {
            int randomNumber = Random.Range(0, randomRange);
            if (!puzzle.Contains(randomNumber))
            {
                puzzle.Add(randomNumber);
            }
        }
        return puzzle;
    }

    public void CheckSolution()
    {
        GameObject[] wheels = GameObject.FindGameObjectsWithTag("wheel");
        List<GameObject> wheelsList = wheels.ToList<GameObject>();
        int cowsCount = 0;
        int bullsCount = CalculateBullsCount(wheelsList);
        if (bullsCount == 4)
        {
            CleanAttemptsBoard();
            NextLevel();
        } else
        {
            cowsCount = CalculateCowsCount(wheelsList, bullsCount);
            UpdateAttemptsBoard(wheelsList, bullsCount, cowsCount);
            timeManager.ReduceTimerTime();
        }
    }

    private int CalculateBullsCount(List<GameObject> wheelsList)
    {
        int bullsCount = 0;
        for (int i = 0; i < wheelsList.Count; i++)
        {
            GameObject currentWheel = GameObject.Find("Wheel " + i);
            if (puzzle[i].Equals(int.Parse(currentWheel.transform.GetChild(0).GetComponent<Text>().text)))
            {
                bullsCount++;
            }
        }
        return bullsCount;
    }

    private int CalculateCowsCount(List<GameObject> wheelsList, int bullsCount)
    {
        int cowsCount = 0;
        for (int i = 0; i < wheelsList.Count; i++)
        {
            GameObject currentWheel = GameObject.Find("Wheel " + i);
            if (puzzle.Contains((int.Parse(currentWheel.transform.GetChild(0).GetComponent<Text>().text))))
            {
                cowsCount++;
            }
        }
        return cowsCount - bullsCount;
    }

    private void UpdateAttemptsBoard(List<GameObject> wheelsList, int bullsCount, int cowsCount)
    {
        string userInput = "";
        for (int i = 0; i < wheelsList.Count; i++)
        {
            GameObject currentWheel = GameObject.Find("Wheel " + i);
            userInput += currentWheel.transform.GetChild(0).GetComponent<Text>().text;
        }
        userInput += " - " + bullsCount + " b. " + cowsCount + " c. \n";
        attemptsBoard.text += userInput;
    }

    private void CleanAttemptsBoard()
    {
        attemptsBoard.text = "";
    }

    private void NextLevel()
    {
        GameObject[] wheelsArray = GameObject.FindGameObjectsWithTag("wheel");
        foreach (GameObject wheel in wheelsArray)
        {
            wheel.transform.GetChild(0).GetComponent<Text>().text = "0";
            CleanAttemptsBoard();
            submitButton.GetComponent<Button>().interactable = false;

            //Implementation of interactable wheels depending ot teammates' progress
            wheel.GetComponent<Button>().interactable = false;
            wheelsForNextLevel = wheelsArray;
        }
        currentLevel++;
        if (currentLevel>= 4)
        {
            timeManager.RemoveWaterFromScene();
            //TODO win condition
            //TODO CHECK IF OTHERS ARE ALREADY THERE
            print("GAME WON");
        }
        localPlayer.GetComponent<PlayerServerCommunication>().SetCurrentLevel(currentLevel);
        GenerateNewPuzzle();
        startTime = Time.time;
        swapBackground = true;
        newBackgroundPosition = new Vector3(backgrounds.transform.position.x, backgrounds.transform.position.y - 10f, backgrounds.transform.position.z);

        timeManager.ResetWaveLevel();
    }

    void Update()
    {
        if (swapBackground)
        {
            wheels.SetActive(false);
            submitButton.SetActive(false);
            attemptsBoard.transform.parent.gameObject.SetActive(false);
            backgrounds.transform.position = Vector3.Lerp(backgrounds.transform.position, newBackgroundPosition, (Time.time - startTime) / 100f);
        }
        if (swapBackground && newBackgroundPosition.y + 0.1f > backgrounds.transform.position.y)
        {
            backgrounds.transform.position = newBackgroundPosition;
            swapBackground = false;
            if (backgrounds.transform.position.y >= -29.5)
            {
                wheels.SetActive(true);
                submitButton.SetActive(true);
                attemptsBoard.transform.parent.gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            NextLevel();
        }

        //Unlock wheels based on teammates' progress
        GameObject networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        ConnectedPlayers connectedPlayers = networkManager.GetComponent<ConnectedPlayers>();
        List<GameObject> teamList;
        List<GameObject> filteredTeamList = new List<GameObject>();

        if (localPlayer.GetComponent<PlayerServerCommunication>().isTeamA) {
            teamList = connectedPlayers.GetOrderedPlayersFromTeamA();
        } else {
            teamList = connectedPlayers.GetOrderedPlayersFromTeamB();
        } foreach (GameObject player in teamList) {
            if (!player.Equals(localPlayer)) {
                filteredTeamList.Add(player);
            }
        }

        int playersReadyBeforeMe = 0;
        foreach (GameObject teammate in filteredTeamList) {
            if (teammate.GetComponent<PlayerServerCommunication>().level == currentLevel) {
                playersReadyBeforeMe++;
            }
        }
        unlockWheels(currentLevel, playersReadyBeforeMe);
    }

    private void unlockWheels(int level, int playersCount)
    {
        if (level == 2) {
            if (playersCount == 1) {
                wheelsForNextLevel[0].SetActive(true);
                wheelsForNextLevel[3].SetActive(true);
            } else {
                wheelsForNextLevel[0].SetActive(true);
                wheelsForNextLevel[1].SetActive(true);
                wheelsForNextLevel[2].SetActive(true);
                wheelsForNextLevel[3].SetActive(true);
            }
        } else if (level == 3) {
            if (playersCount == 1) {
                wheelsForNextLevel[1].SetActive(true);
                wheelsForNextLevel[3].SetActive(true);
            } else {
                wheelsForNextLevel[0].SetActive(true);
                wheelsForNextLevel[1].SetActive(true);
                wheelsForNextLevel[2].SetActive(true);
                wheelsForNextLevel[3].SetActive(true);
            }
        }
    }

    public void IncrementWheelValue() {
        GameObject wheel = EventSystem.current.currentSelectedGameObject;
        int currentValue = int.Parse(wheel.transform.GetChild(0).GetComponent<Text>().text);
        currentValue++;
        int maxValue;
        switch (currentLevel)
        {
            default:
            case 1:
                maxValue = 5;
                break;
            case 2:
                maxValue = 7;
                break;
            case 3:
                maxValue = 9;
                break;
        }
        if (currentValue > maxValue)
        {
            currentValue = 0;
        }
        wheel.transform.GetChild(0).GetComponent<Text>().text = currentValue.ToString();
        int occurencesCount = GetOccurencesCount();
        if (occurencesCount <= 1)
        {
            submitButton.GetComponent<Button>().interactable = true;
        } else
        {
            submitButton.GetComponent<Button>().interactable = false;
        }
    }

    private int GetOccurencesCount()
    {
        int maxOccurencesCount = 0;
        int currentOccurences = 0;
        GameObject[] wheels = GameObject.FindGameObjectsWithTag("wheel");
        foreach (GameObject currentWheel in wheels)
        {
            currentOccurences = 0;
            int currentWheelValue = int.Parse(currentWheel.transform.GetChild(0).GetComponent<Text>().text);
            foreach (GameObject wheelInSecondLoop in wheels)
            {
                if (currentWheelValue == int.Parse(wheelInSecondLoop.transform.GetChild(0).GetComponent<Text>().text))
                currentOccurences++;
            }
            if (currentOccurences > maxOccurencesCount)
            {
                maxOccurencesCount = currentOccurences;
            }
        }
        return maxOccurencesCount;
    }

    public void setLocalPlayer(GameObject player)
    {
        localPlayer = player;
    }

}
