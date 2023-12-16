using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    [SerializeField] private int baseSceneIndex;
    private static float originTimeScale;    

    // [Header("Transition Settings")]
    // [SerializeField] private GameObject[] fadeObjects;

    // enum TransitionType {
    //     FadeIn,
    //     FadeOut,
    //     SlideIn,
    //     SlideOut,
    //     ZoomIn,
    //     ZoomOut,
    //     RotateIn,
    //     RotateOut,
    //     ScaleIn,
    //     ScaleOut,
    //     None,
    // }
    // private void DoTransition(TransitionType type) {
    //     switch (type) {
    //     case TransitionType.FadeIn:
    //         foreach (GameObject gObj in fadeObjects) {
    //             StartCoroutine(Fade(gObj, 1f, true));
    //         }
    //         break;
    //     case TransitionType.FadeOut: 
    //         foreach (GameObject gObj in fadeObjects) {
    //             StartCoroutine(Fade(gObj, 1f, false));
    //         }
    //         break;
    //     case TransitionType.SlideIn:
    //         break;
    //     case TransitionType.SlideOut:
    //         break;
    //     case TransitionType.ZoomIn:
    //         break;
    //     case TransitionType.ZoomOut:
    //         break;
    //     case TransitionType.RotateIn:
    //         break;
    //     case TransitionType.RotateOut:
    //         break;
    //     case TransitionType.ScaleIn:
    //         break;
    //     case TransitionType.ScaleOut:
    //         break;
    //     case TransitionType.None:
    //         break;
    //     }
    // }

    // private IEnumerator Fade(GameObject gObj, float fadeDuration, bool fadeIn)
    // {
    //     Renderer rend = gObj.transform.GetComponent<Renderer>();
        
    //     Color initialColor = rend.material.color;
    //     Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, fadeIn ? 1f : 0f);

    //     float elapsedTime = 0f;

    //     while (elapsedTime < fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         rend.material.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
    //         yield return null;
    //     }
    // }

    void Awake(){
        if (instance == null)
        {
            instance = this;
            originTimeScale = Time.timeScale;

            DontDestroyOnLoad(this);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public static LevelManager GetInstance()
    {
        return instance;
    }

    public int GetLevelOffset()
    {
        return levelOffset;
    }
    public int GetLevel()
    {
        return selectedLevel;
    }

    public void SetLevel(int level)
    {
        selectedLevel = level;
    }

    public void LoadOptionScene()
    {
        SceneManager.LoadScene("OptionMenu", LoadSceneMode.Additive);

    }
    public void ExitOptionScene()
    {
        SceneManager.UnloadSceneAsync("OptionMenu");
    } 

    public void LoadPausedScene()
    {
        SceneManager.LoadScene("PausedMenu", LoadSceneMode.Additive);
        originTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void ExitPausedScene()
    {
        SceneManager.UnloadSceneAsync("PausedMenu");
        Time.timeScale = originTimeScale;
    } 

    /// <summary>
    /// Loads the LevelMenu scene
    /// </summary>
    public void LoadLevelMenuScene(){
        string s = "LevelMenu";
        StartCoroutine(LoadSceneStr(s));
    } 
    public void LoadTutorialLevelScene(){
        string s = "Tutorial";
        StartCoroutine(LoadSceneStr(s));
    } 
    public void ExitLevelMenuScene(){
        string s = "MainMenu";
        StartCoroutine(LoadSceneStr(s));
    }

    public void LoadScene(string string_name)
    {
        SceneManager.LoadScene(string_name);
    }

    /// <summary>
    /// Level Play Controller
    /// `selectedLevel` is the index of the level to be loaded
    /// `levelOffset` is the index of the first level in the build settings
    /// For example, if the first-idx[0] in the build settings is the main menu, and the third-idx[2] is the first level of the game, then `levelOffset` should be 2.
    /// 
    /// </summary>
    [Header("Level Play Controller")]
    [SerializeField] private int selectedLevel;
    [SerializeField] private int levelOffset; 

    public void LoadScene(int int_index){
        int i = int_index;
        StartCoroutine(LoadSceneInt(i));

    }
    public void LoadLevel(int int_index){
        int i = int_index + levelOffset;
        StartCoroutine(LoadSceneInt(i));
    }
    public void LoadNextScene(){
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadSceneInt(i));

    }
    public void ReloadScene(){
        int i = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadSceneInt(i));
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnloadScene(string string_name)
    {
        SceneManager.UnloadSceneAsync(string_name);
    }

    IEnumerator LoadSceneInt(int i)
    {
        Transition.GetInstance().StartTransition(true, Color.black, 2f, 1f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(i);
    }

    IEnumerator LoadSceneStr(string s)
    {
        Transition.GetInstance().StartTransition(true, Color.black, 2f, 1f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(s);
    }
}

