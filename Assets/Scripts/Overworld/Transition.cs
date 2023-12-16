 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TransitionType{
    Fade,
    Circular,
}

public class Transition : MonoBehaviour
{
    private static Transition instance;
    [SerializeField] private float fadeDuration;
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private Image image;

    private Color[] currentColor {
        get {
            return new Color[] {
                image.color,
            };
        }
        set {
            image.color = value[0];
        } 
    }
    private Color[] originalColor;

    private void Awake()
    {
        Debug.Log("Transition Awake");
        if (instance == null)
        {
            instance = this;

            Initialize();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("Transition Start");
        Reset();
        Debug.Log("Transition Start<end>");
    }

    public static Transition GetInstance()
    {
        return instance;
    }

    ///<summary>
    /// Fades the screen to black or clear
    /// </summary>
    /// <param name="toClearNotBlack">True if fading to clear, false if fading to black</param>
    /// <param name="duration">Duration of the fade</param>
    /// <param name="delay">Delay before the fade starts</param>
    /// <param name="callback">Callback function to be called after the fade</param>
    /// <returns></returns>
    ///     

    private bool isTransitionOut;

    IEnumerator Fade(Color to, float duration, float delay, System.Action callback = null)
    {

        gameObject.SetActive(true);

        image.fillAmount = 1f;
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;
        Color[] froms = new Color[3];
        froms = originalColor;
        Color[] tos = new[] { to, to, to };

        Color[] temp = new Color[3];
        float t = 0;
        while (t < duration)
        {
            
            for (int i = 0; i < 1; i++) {

                temp[i] = Color.Lerp(froms[i], tos[i], t);
            }
            currentColor = temp;
            yield return null;
            t = (Time.time - startTime) / duration;
        }

        gameObject.SetActive(isTransitionOut);

        yield return null;
    }

    IEnumerator Circular(Color to, float duration, float delay,System.Action callback = null)
    {
        gameObject.SetActive(true);

        image.fillAmount = 1f;
        yield return new WaitForSeconds(delay);

        Color[] froms = new Color[3];
        froms = originalColor;
        Color[] temp = new Color[3];
        temp = originalColor;

        Color[] tos = new[] { to, to, to };

        float t = 0;
        while (t < duration)
        {
            for (int i = 0; i < 3; i++) {
                temp[i] = Color.Lerp(froms[i], tos[i], t/duration);
            }
            temp[0] = originalColor[0]; // skip image.color
            currentColor = temp;
            image.fillAmount = Mathf.Lerp(1f, 0f, t/duration);
            yield return null;
            t += Time.deltaTime;
            for (int i = 0; i < 3; i++) {
                Color col = currentColor[i];
                Debug.Log(col.r + " " + col.g + " " + col.b + " " + col.a);
            }
            Debug.Log(t);

        }
        gameObject.SetActive(isTransitionOut);

        yield return null;
    }

    public void ResetColor()
    {
        currentColor = originalColor;
    }

    public void StartTransition(bool isTransitionOut, Color to, float duration, float delay = 0f, TransitionType type = TransitionType.Fade, System.Action callback = null)
    {
        this.isTransitionOut = isTransitionOut;
        switch(type){
        case TransitionType.Fade:
            StartCoroutine(Fade(to, duration, delay));
            break;

        case TransitionType.Circular:
            StartCoroutine(Circular(to, duration, delay));
            break;

        default:
            StartCoroutine(Fade(to, duration, delay));

            break;
        }
    }

    private void Reset()
    {
        ResetColor();
        StartTransition(false, Color.clear, 2f, 1.5f, TransitionType.Fade);
    }

    private void Initialize(){
        image = GetComponent<Image>();

        originalColor = new Color[3];
        originalColor = currentColor;
    }
}
