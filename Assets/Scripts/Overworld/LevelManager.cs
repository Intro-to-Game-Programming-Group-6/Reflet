using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;



public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentSceneIndex;
    public static LevelManager Instance;
    private static float originTimeScale;


    void Awake(){
        if (Instance == null)
        {
            Instance = this;
            originTimeScale = Time.timeScale;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    public static LevelManager GetInstance()
    {
        return Instance;
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
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
        originTimeScale = Time.timeScale;
        Time.timeScale = 0.05f;
        SceneManager.LoadScene("PausedMenu", LoadSceneMode.Additive);
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().enabled = false;
    }

    public void ExitPausedScene()
    {
        SceneManager.UnloadSceneAsync("PausedMenu");
        Time.timeScale = originTimeScale;
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().enabled = true;
    } 

<<<<<<< HEAD
    public void LoadLevelMenuScene(){
        string s = "LevelMenu";
        StartCoroutine(TransitionLoadScene(s));
    } 
    public void LoadTutorialLevelScene(){
        string s = "Tutorial";
        StartCoroutine(TransitionLoadScene(s));
    } 
    public void ExitLevelMenuScene(){
        string s = "MainMenu";
        StartCoroutine(TransitionLoadScene(s));
=======
    public void LoadTutorialLevelScene(){
        string s = "Scenes/Play/Tutorial";
        StartCoroutine(LoadSceneStr(s));
    } 
    public void ExitLevelMenuScene(){
        string s = "Scenes/Menu/MainMenu";
        StartCoroutine(LoadSceneStr(s));
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
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

    public void SetLevel(int lvl){
        selectedLevel = lvl;
    }
    public void LoadScene(int int_index){
        int i = int_index;
        StartCoroutine(TransitionLoadScene(i));

    }
    public void LoadLevel(int int_index){
        int i = int_index + levelOffset;
        StartCoroutine(TransitionLoadScene(i));
    }
    public void LoadNextScene(){
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(TransitionLoadScene(i));

    }
    public void ReloadScene(){
        int i = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(TransitionLoadScene(i));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { Application.Quit(); return; }
        else { LoadMainMenu(); return; }
    }

    IEnumerator TransitionLoadScene(int i , float transitironDuration = 1.5f, float delay = 1.0f)
    {
<<<<<<< HEAD
        Transition.GetInstance().StartTransition(Color.black, transitironDuration, delay);
        yield return new WaitForSeconds(transitironDuration + delay);
=======
        SceneManager.UnloadSceneAsync(string_name);
    }

    IEnumerator LoadSceneInt(int i)
    {
        Transition.GetInstance().StartFade(Color.black, 1.5f, 1f);
        yield return new WaitForSeconds(2f);
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
        SceneManager.LoadScene(i);
    }

    IEnumerator TransitionLoadScene(string s, float transitironDuration = 1.5f, float delay = 1.0f)
    {
<<<<<<< HEAD
        Transition.GetInstance().StartTransition(Color.black, transitironDuration, delay);
        yield return new WaitForSeconds(transitironDuration + delay);
=======
        Transition.GetInstance().StartFade(Color.black, 1.5f, 1f);
        yield return new WaitForSeconds(2f);
>>>>>>> 0df300983e3fadecfd5729cf06bbbc1651af8248
        SceneManager.LoadScene(s);
    }
}

