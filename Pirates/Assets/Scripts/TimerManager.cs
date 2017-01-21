﻿using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    [SerializeField]
    private float timeLeft;

    [SerializeField]
    private float timePenaltyOnError;

    [SerializeField]
    private GameObject cowsBullsManagerObject;

    [SerializeField]
    private GameObject waveEffect;

    private CowsBullsManager cowsBullsManager;
    private float initialTimerValue;
    private float timeElapsedPercent;
    private float totalWavePath;
    private float initialWaveLevel;
    private float finalWaveLevel;

    void Start()
    {
        initialTimerValue = timeLeft;
        cowsBullsManager = cowsBullsManagerObject.GetComponent<CowsBullsManager>();
        initialWaveLevel = -232f;
        finalWaveLevel = 518f;
        totalWavePath = Mathf.Abs(initialWaveLevel - finalWaveLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReduceTimerTime();
        }

        timeElapsedPercent = (initialTimerValue - timeLeft) / initialTimerValue * 100;
        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            UpdateWaveLevel();
        }
    }

    public void ReduceTimerTime()
    {
        timeLeft -= timePenaltyOnError;
    }

    private void UpdateWaveLevel()
    {
        waveEffect.transform.position = new Vector3(waveEffect.transform.position.x, 
            timeElapsedPercent * totalWavePath / 100 + initialWaveLevel, waveEffect.transform.position.z);
    }

    public void ResetWaveLevel()
    {
        Invoke("ResetWater", 3.5f);
    }

    private void ResetWater()
    {
        timeLeft = initialTimerValue;
        timeElapsedPercent = 0;
        waveEffect.transform.position = new Vector3(waveEffect.transform.position.x, initialWaveLevel, waveEffect.transform.position.z);
    }
}
