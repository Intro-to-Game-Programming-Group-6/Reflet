using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;


public class LevelManager : MonoBehaviour
{
    private static LevelManager Instance;
    [SerializeField] private int baseSceneIndex;
    private static float originTimeScale;

    [Header("Level Play Controller")]
    [SerializeField] private int selectedLevel;
    [SerializeField] private int levelOffset; 

    [Header("Randomizer Components")]
    EnemyManager enemyManager;
    List<GameObject> selectedEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> EnemyPrefabs = new List<GameObject>();
    [SerializeField] private List<string> PlayMaps = new List<string>();
    private Queue<string> sceneQueue = new Queue<string>();
    private int maxQueueLen = 1;
    // private int finalBossThresh = 10;

    void Awake()
    {
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

    #region  Randomizer
    public void Reset()
    {
        if(EnemyManager.GetInstance() != null)
        {
            enemyManager = EnemyManager.GetInstance();
            
            selectedEnemies.Clear();
            selectedEnemies.Add(EnemyPrefabs[0]);

            int enemyLimit = 1;
            int enemyTotal = 1;

            if(SceneManager.GetActiveScene().name != "Tutorial")
            {
                SelectRandomEnemies();
                enemyLimit = 5;
                enemyTotal = 10;
            }

            enemyManager.SetEnemySelections(selectedEnemies, enemyLimit, enemyTotal);
        }
    }

    void SelectRandomEnemies()
    {
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(0, EnemyPrefabs.Count);

            selectedEnemies.Add(EnemyPrefabs[randomIndex]);
        }
    }

    public string SelectRandomScene()
    {
        string selectedScene = null;

        if (PlayMaps.Count > 0)
        {
            while (selectedScene == null || sceneQueue.Contains(selectedScene))
            {
                selectedScene = PlayMaps[Random.Range(0, PlayMaps.Count)];
            }
        }

        return selectedScene;
    }

    public void Enqueue(string value)
    {
        if (sceneQueue.Count >= maxQueueLen)
        {
            sceneQueue.Dequeue();
        }
        sceneQueue.Enqueue(value);
    }

    #endregion

    #region Level Transition
    public int GetLevelOffset()
    {
        return levelOffset;
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
    } 

    public void LoadTutorialLevelScene()
    {
        string s = "Scenes/Play/Tutorial";
        StartCoroutine(TransitionLoadScene(s));
    } 
    public void ExitLevelMenuScene()
    {
        string s = "Scenes/Menu/MainMenu";
        StartCoroutine(TransitionLoadScene(s));
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
        Transition.GetInstance().transitionDelay = delay;
        Transition.GetInstance().transitionDuration = transitironDuration;
        Transition.GetInstance().Exit(); 
        yield return new WaitForSeconds(transitironDuration + delay);
        SceneManager.LoadScene(i);
    }

    IEnumerator TransitionLoadScene(string s, float transitironDuration = 1.5f, float delay = 1.0f)
    {
        Transition.GetInstance().transitionDelay = delay;
        Transition.GetInstance().transitionDuration = transitironDuration;
        Transition.GetInstance().Exit();
        yield return new WaitForSeconds(transitironDuration + delay);
        SceneManager.LoadScene(s);
    }
    #endregion
}

