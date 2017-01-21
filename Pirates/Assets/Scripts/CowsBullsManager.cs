using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class CowsBullsManager : MonoBehaviour {

    [SerializeField]
    private int currentLevel = 1;

    [SerializeField]
    GameObject backgrounds;

    [SerializeField]
    GameObject wheels;

    [SerializeField]
    GameObject submitButton;

    [SerializeField]
    List<int> puzzle;

    private float startTime;
    private bool swapBackground = false;
    private Vector3 newBackgroundPosition;

    private void Start()
    {
        submitButton.GetComponent<Button>().interactable = false;
        GenerateNewPuzzle();
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
        int bullsCount = CalculateBullsCount(wheelsList);
        print("BULLS: " + bullsCount);
        if (bullsCount == 4)
        {
            NextLevel();
        } else
        {
            int cowsCount = CalculateCowsCount(wheelsList, bullsCount);
            print("COWS: " + cowsCount);
        }
        UpdateAttemptsBoard();
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
            print("Check " + puzzle[i] + " and " + currentWheel.transform.GetChild(0).GetComponent<Text>().text);
            if (puzzle.Contains((int.Parse(currentWheel.transform.GetChild(0).GetComponent<Text>().text))))
            {
                cowsCount++;
            }
        }
        return cowsCount - bullsCount;
    }

    private void UpdateAttemptsBoard()
    {
        //TODO
    }

    private void NextLevel()
    {
        currentLevel++;
        GenerateNewPuzzle();
        startTime = Time.time;
        swapBackground = true;
        newBackgroundPosition = new Vector3(backgrounds.transform.position.x, backgrounds.transform.position.y - 10f, backgrounds.transform.position.z);
    }

    void Update()
    {
        if (swapBackground)
        {
            wheels.SetActive(false);
            submitButton.SetActive(false);
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
            }
        }
    }

    private void UpdateWheelsUI() {
        GameObject[] wheels = GameObject.FindGameObjectsWithTag("wheel");
        List<GameObject> wheelsList = wheels.ToList<GameObject>();
        if (currentLevel == 1)
        {
            wheelsList.Reverse();
        }
        for (int i = 0; i <= wheelsList.Count - 1; i++)
        {
            wheelsList[i].transform.GetChild(0).GetComponent<Text>().text = puzzle[i].ToString();
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

}
