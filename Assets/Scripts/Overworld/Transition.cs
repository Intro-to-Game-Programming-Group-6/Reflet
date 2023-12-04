using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    private static Transition instance;
    private Image image;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            image = GetComponent<Image>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Reset();
    }

    public static Transition GetInstance()
    {
        return instance;
    }

    IEnumerator Fade(Color to, float duration, float delay, System.Action callback = null)
    {
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;
        Color from = image.color;
        
        float t = 0;

        while (t < 1)
        {
            image.color = Color.Lerp(from, to, t);
            yield return null;
            t = (Time.time - startTime) / duration;
        }

        yield return null;
    }

    public void SetColor(Color to)
    {
        image.color = to;
    }

    public void StartFade(Color to, float duration, float delay = 0f, System.Action callback = null)
    {
        StartCoroutine(Fade(to, duration, delay));
    }

    private void Reset()
    {
        SetColor(Color.black);
        StartFade(Color.clear, 1f, 1f);
    }
}
