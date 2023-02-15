using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum QTStates
{
    INACTIVE,
    START,
    IN_BUFFER,
    AWAITING_INPUT,
    END
}
public class QuickTimeEvent : MonoBehaviour
{
    public static QuickTimeEvent instance;
    [SerializeField] private KeyCode currentKeycode;
    [SerializeField] private QTStates state = QTStates.INACTIVE;
    [SerializeField] private QTEData data;
    [SerializeField, Range(0f, 1f)] private float successMeter;
    [SerializeField] private int waveSuccess;

    private WaitForSeconds bufferWait;
    private bool stopDecay;
    private bool stopWaitingForInput;

    private void Awake()
    {
        if (instance != this)
        {
            if (instance != null)
                Destroy(instance);
            instance = this;
        }
        Init();
    }

    private void Init()
    {
        bufferWait = new WaitForSeconds(data.bufferTime);
    }

    private void Start()
    {
        StartQTEWrapper();
    }

    public void StartQTEWrapper()
    {
        state = QTStates.START;
        StartCoroutine(StartQTE());
    }

    public IEnumerator StartQTE()
    {
        for (int i = 0; i < data.waves; i++)
        {
            yield return StartWave();
        }
        state = QTStates.END;
        Debug.Log(waveSuccess >= data.winsRequired);
    }

    private IEnumerator StartWave()
    {
        state = QTStates.IN_BUFFER;
        currentKeycode = GameConfig.Constants.QTEKeys[Mathf.CeilToInt(Random.Range(0, GameConfig.Constants.QTEKeys.Length))];
        yield return bufferWait;

        StartDecay();
        float elapsedTime = 0f;
        state = QTStates.AWAITING_INPUT;
        while (elapsedTime <= data.timeAllowedPerWave)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if (stopWaitingForInput)
            {
                stopWaitingForInput = false;
                WaveSuccess();
                yield break;
            }
        }
        WaveFail();
    }

    public void StartDecay()
    {
        successMeter = 0.3f;
        stopDecay = false;
    }
    private void Update()
    {
        if (state != QTStates.AWAITING_INPUT) return;

        if (successMeter > 0f&&!stopDecay)
        {
            successMeter -= Time.deltaTime * data.successDecayRate;
        }

        if (Input.GetKeyDown(currentKeycode))
        {
            successMeter += data.correctHitIncrement;
            if (successMeter > 1f)
            {
                stopWaitingForInput = true;
            }
        }
    }

    private void WaveFail()
    {
        stopDecay = true;
    }

    private void WaveSuccess()
    {
        stopDecay = true;
        waveSuccess++;
    }
}
