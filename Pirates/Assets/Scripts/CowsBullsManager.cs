using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CowsBullsManager : MonoBehaviour {

    [SerializeField]
    private int currentLevel = 1;

    [SerializeField]
    GameObject backgrounds;

    [SerializeField]
    GameObject wheels;

    private float startTime;
    private bool swapBackground = false;
    private Vector3 newBackgroundPosition;

    public void CheckSolution()
    {
        print("Very cool. Now get drunk!");
        if (true)   //TODO
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        currentLevel++;
        startTime = Time.time;
        swapBackground = true;
        newBackgroundPosition = new Vector3(backgrounds.transform.position.x, backgrounds.transform.position.y - 10f, backgrounds.transform.position.z);
    }

    void Update()
    {
        if (swapBackground)
        {
            wheels.SetActive(false);
            backgrounds.transform.position = Vector3.Lerp(backgrounds.transform.position, newBackgroundPosition, (Time.time - startTime) / 100f);
        }
        if (newBackgroundPosition.y + 0.1f > backgrounds.transform.position.y)
        {
            backgrounds.transform.position = newBackgroundPosition;
            swapBackground = false;
            wheels.SetActive(true);
        }
    }
}
