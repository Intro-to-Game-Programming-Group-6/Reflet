using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    private static Transition instance;
    [SerializeField] private float transitionDuration;
    [SerializeField] private Image image;
    [SerializeField] private Text title1;
    [SerializeField] private Text title2;

<<<<<<< HEAD
    private Color[] currentColor {
        get {
            return new Color[] {
                image.color,
                title1.color,
                title2.color
            };
        }
        set {
            image.color = value[0];
            title1.color = value[1];
            title2.color = value[2];
        } 
    }
    private Color[] originalColor;

=======
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
<<<<<<< HEAD
            Initialize();
=======
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
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

<<<<<<< HEAD
    ///<summary>
    /// Fades the screen to black or clear
    /// </summary>
    /// <param name="duration">Duration of the fade</param>
    /// <param name="delay">Delay before the fade starts</param>
    /// <param name="callback">Callback function to be called after the fade</param>
    /// <returns></returns>
    ///     

    IEnumerator Fade(Color? to, float duration, float delay, System.Action callback = null)
    {

        image.fillAmount = 1f;
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;
        Color[] froms = new Color[3];
        froms = currentColor;
        Color[] tos;
        if (to == null){
            tos = originalColor;
        }else{
            Color colorTemp = to ?? Color.clear;
            tos = new[] {colorTemp, colorTemp, colorTemp};
        }

        Color[] temp = new Color[3];
        float t = 0;
        while (t < duration)
        {
            
            for (int i = 0; i < 3; i++) {
=======
    IEnumerator Fade(Color to, float duration, float delay, System.Action callback = null)
    {
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;
        Color from = image.color;
        
        float t = 0;
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248

        while (t < 1)
        {
            image.color = Color.Lerp(from, to, t);
            yield return null;
            t = (Time.time - startTime) / duration;
        }
        title1.text = "";
        title2.text = "";

<<<<<<< HEAD
        if (callback != null) callback.Invoke();
        yield return null;
    }

    IEnumerator Circular(Color? to, float duration, float delay, System.Action callback = null)
    {
        image.fillAmount = 1f;
        yield return new WaitForSeconds(delay);

        Color[] froms = new Color[3];
        froms = currentColor;
        Color[] temp = new Color[3];
        temp = originalColor;
        Color[] tos;
        if (to == null){
            tos = originalColor;
        }else{
            Color colorTemp = to ?? Color.clear;
            tos = new[] {colorTemp, colorTemp, colorTemp};
        }

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
        title1.text = "";
        title2.text = "";

        if (callback != null) callback.Invoke();
        yield return null;
=======
        yield return null;
    }

    public void SetColor(Color to)
    {
        image.color = to;
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
    }

    public void StartFade(Color to, float duration, float delay = 0f, System.Action callback = null)
    {
<<<<<<< HEAD
        currentColor = originalColor;
    }

    public void StartTransition(Color? to, float duration, float delay = 0f, TransitionType type = TransitionType.Fade, System.Action callback = null)
    {
        switch(type){
        case TransitionType.Fade:
            StartCoroutine(Fade(to, duration, delay, callback));
            break;

        case TransitionType.Circular:
            StartCoroutine(Circular(to, duration, delay, callback));
            break;

        default:
            StartCoroutine(Fade(to, duration, delay, callback));
            break;
        }
=======
        StartCoroutine(Fade(to, duration, delay));
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
    }

    private void Reset()
    {
<<<<<<< HEAD
        ResetColor();
        StartTransition(Color.clear, 2f, 1.5f, TransitionType.Fade);
    }

    private void Initialize(){
        image = GetComponent<Image>();

        originalColor = new Color[3];
        originalColor = currentColor;
=======
        SetColor(Color.black);
        StartFade(Color.clear, 1f, 1f);
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
    }
}
