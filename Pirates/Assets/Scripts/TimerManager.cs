using UnityEngine;
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

    [SerializeField]
    private AudioClip levelFailedSfx;

    private CowsBullsManager cowsBullsManager;
    private float initialTimerValue;
    private float timeElapsedPercent;
    private float totalWavePath;
    private float initialWaveLevel;
    private float finalWaveLevel;
    private AudioSource audio;

    //Move wave horizontally
    float speed = 2f;
    float startTime, journeyLength, distCovered, fracJourney;
    Vector3 waveEffectPosition, randomPosition;
    Vector3 waveEffectPosition2, randomPosition2;
    Quaternion waveEffectRotation, randomRotation;
    Quaternion waveEffectRotation2, randomRotation2;

    void Start()
    {
        initialTimerValue = timeLeft;
        cowsBullsManager = cowsBullsManagerObject.GetComponent<CowsBullsManager>();
        initialWaveLevel = -232f;
        finalWaveLevel = 518f;
        totalWavePath = Mathf.Abs(initialWaveLevel - finalWaveLevel);
        audio = GetComponent<AudioSource>();

        InvokeRepeating("MoveWaveHorizontally", 0, 3);
        InvokeRepeating("MoveWaveHorizontally2", 0, 4);
    }

    private void MoveWaveHorizontally()
    {
        startTime = Time.time;
        waveEffectPosition = waveEffect.transform.position;
        waveEffectRotation = waveEffect.transform.rotation;
        randomPosition = new Vector3(Random.Range(-100f, 1000f), waveEffectPosition.y, waveEffectPosition.z);
        journeyLength = Vector3.Distance(waveEffectPosition, randomPosition);
        randomRotation = new Quaternion(waveEffectRotation.x, waveEffectRotation.y, Random.Range(-0.1f, 0.1f), waveEffectRotation.w);
    }

    private void MoveWaveHorizontally2()
    {
        startTime = Time.time;
        waveEffectPosition2 = waveEffect.transform.GetChild(0).transform.position;
        waveEffectRotation2 = waveEffect.transform.GetChild(0).transform.rotation;
        randomPosition2 = new Vector3(Random.Range(-100f, 1000f), waveEffectPosition2.y, waveEffectPosition2.z);
        journeyLength = Vector3.Distance(waveEffectPosition2, randomPosition2);
        randomRotation2 = new Quaternion(waveEffectRotation2.x, waveEffectRotation2.y, Random.Range(-0.1f, 0.1f), waveEffectRotation2.w);
    }

    void Update()
    {
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        waveEffect.transform.position = Vector3.Lerp(waveEffect.transform.position, randomPosition, fracJourney);
        waveEffect.transform.rotation = Quaternion.Lerp(waveEffect.transform.rotation, randomRotation, fracJourney);

        waveEffect.transform.GetChild(0).transform.position = Vector3.Lerp(waveEffect.transform.GetChild(0).transform.position, randomPosition2, fracJourney);
        waveEffect.transform.GetChild(0).transform.rotation = Quaternion.Lerp(waveEffect.transform.GetChild(0).transform.rotation, randomRotation2, fracJourney);

        if (Input.GetKeyDown(KeyCode.P))
        {
            ReduceTimerTime();
        }

        timeElapsedPercent = (initialTimerValue - timeLeft) / initialTimerValue * 100;
        timeLeft -= Time.deltaTime;

        if (timeLeft > 0)
        {
            UpdateWaveLevel();
        } else
        {
            audio.PlayOneShot(levelFailedSfx);
            GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>().GoToState(GameState.State.Lose);
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
        waveEffect.transform.GetChild(0).transform.position = new Vector3(waveEffect.transform.GetChild(0).transform.position.x,
            timeElapsedPercent * totalWavePath / 100 + initialWaveLevel, waveEffect.transform.GetChild(0).transform.position.z);
    }

    public void ResetWaveLevel()
    {
        Invoke("ResetWater", 1.5f);
    }

    private void ResetWater()
    {
        timeLeft = initialTimerValue;
        timeElapsedPercent = 0;
        waveEffect.transform.position = new Vector3(waveEffect.transform.position.x, initialWaveLevel, waveEffect.transform.position.z);
        waveEffect.transform.GetChild(0).transform.position = new Vector3(waveEffect.transform.GetChild(0).transform.position.x, initialWaveLevel, waveEffect.transform.GetChild(0).transform.position.z);

    }

    public void RemoveWaterFromScene()
    {
        waveEffect.SetActive(false);
        waveEffect.transform.GetChild(0).transform.gameObject.SetActive(false);
    }
}
