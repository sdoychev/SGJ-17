using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    [SerializeField]
    private float timeLeft;

    [SerializeField]
    private GameObject cowsBullsManagerObject;

    [SerializeField]
    private Image waveEffectImage;

    private CowsBullsManager cowsBullsManager;

    void Start()
    {
        cowsBullsManager = cowsBullsManagerObject.GetComponent<CowsBullsManager>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        UpdateWaveLevel();
        if (timeLeft < 0)
        {
            print("GAME OVER"); //TODO
        }
    }

    public void ReduceTimerTime()
    {
        //TODO
        UpdateWaveLevel();
    }

    private void UpdateWaveLevel()
    {
        //TODO
    }
}
