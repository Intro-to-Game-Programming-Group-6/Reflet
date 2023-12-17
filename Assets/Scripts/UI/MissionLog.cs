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

    private Vector3 basePosition = new Vector3(0, 0, 0); //new Vector3(290, 280, 0);
    private Vector3 secondaryPosition = new Vector3(0, 0, 0);  //Vector3(290, 400, 0);

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
        basePosition = log.transform.position + new Vector3(0, -80, 0);
        secondaryPosition = log.transform.position + new Vector3(0, 30, 0);
    }

    public IEnumerator UpdateLog(string value)
    {
        isUpdating = true;

        // Variable Initialization
        Vector3 startPosition;
        float elapsedTime;

        // Remove Old Log
        startPosition = log.transform.position;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            log.transform.position = Vector3.Lerp(startPosition, secondaryPosition, t);
            yield return null;
        }

        log.GetComponent<Mission>().UpdateText(value);

        //Update New Log
        startPosition = log.transform.position;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            log.transform.position = Vector3.Lerp(startPosition, basePosition, t);
            yield return null;
        }

        isUpdating = false;
    }
}
