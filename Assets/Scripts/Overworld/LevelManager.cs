using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    [SerializeField] private int baseSceneIndex;
    private static float originTimeScale;

    [Header("Level Play Controller")]
    [SerializeField] private int selectedLevel;
    [SerializeField] private int levelOffset; 

    [Header("Randomizer Components")]
    EnemyManager enemyManager;
    List<GameObject> selectedEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> EnemyPrefabs;// = new List<GameObject>();
    [SerializeField] private List<string> PlayMaps = new List<string>();
    private Queue<string> sceneQueue = new Queue<string>();
    private int maxQueueLen = 1;
    // private int finalBossThresh = 10;

    void Awake()
    {
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

    void Start()
    {
        Enqueue("Map1");
    }

    #region  Randomizer
    public void Reset()
    {
        if(EnemyManager.GetInstance() != null)
        {
            enemyManager = EnemyManager.GetInstance();
            
            selectedEnemies.Clear();
            
            int enemyLimit;
            int enemyTotal;

            if(SceneManager.GetActiveScene().name == "Tutorial")
            {
                selectedEnemies.Add(EnemyPrefabs[0]);
                enemyLimit = 1;
                enemyTotal = 3;
            }
            else
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
        for (int i = 0; i < 3; i++)
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

        Enqueue(selectedScene);

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

    public void LoadTutorialLevelScene()
    {
        string s = "Scenes/Play/Tutorial";
        StartCoroutine(LoadSceneStr(s));
    } 
    public void ExitLevelMenuScene()
    {
        string s = "Scenes/Menu/MainMenu";
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
        Transition.GetInstance().StartFade(Color.black, 1.5f, 1f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(i);
    }

    IEnumerator LoadSceneStr(string s)
    {
        Transition.GetInstance().StartFade(Color.black, 1.5f, 1f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(s);
    }
    #endregion
}

