using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class MissionLog : MonoBehaviour
{
    private static MissionLog instance;

    public GameObject log;

    public float duration;
    public bool isUpdating;

    private Vector3 basePosition;
    private Vector3 secondaryPosition;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static MissionLog GetInstance()
    {
        return instance;
    }

    void Start()
    {
        isUpdating = false;
        basePosition = log.transform.localPosition + new Vector3(0, -200, 0);
        secondaryPosition = log.transform.localPosition + new Vector3(0, 0, 0);
    }

    public IEnumerator UpdateLog(string value)
    {
        isUpdating = true;

        // Variable Initialization
        Vector3 startPosition;
        float elapsedTime;

        // Remove Old Log
        startPosition = log.transform.localPosition;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            log.transform.localPosition = Vector3.Lerp(startPosition, secondaryPosition, t);
            yield return null;
        }

        log.GetComponent<Mission>().UpdateText(value);

        //Update New Log
        startPosition = log.transform.localPosition;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            log.transform.localPosition = Vector3.Lerp(startPosition, basePosition, t);
            yield return null;
        }

        isUpdating = false;
    }
}
