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
    [SerializeField] private List<GameObject> EnemyPrefabs;// = new List<GameObject>();
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
        else
        {
            Debug.LogWarning("EnemyManager is null at this point.");
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

    #region Option Menu
    public void LoadOptionScene()
    {
        SceneManager.LoadScene("OptionMenu", LoadSceneMode.Additive);
    }

    public void ExitOptionScene()
    {
        SceneManager.UnloadSceneAsync("OptionMenu");
    }
    #endregion

    #region Paused Menu
    public void LoadPausedScene()
    {
        originTimeScale = Time.timeScale;
        Time.timeScale = 0.05f; // This determines the animation's speed at PausedMenu. For example 0.05f := 1/20, thus the speed up is 20x.
        SceneManager.LoadScene("PausedMenu", LoadSceneMode.Additive);
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().enabled = false;
    }

    public void ExitPausedScene()
    {
        SceneManager.UnloadSceneAsync("PausedMenu");
        Time.timeScale = originTimeScale;
        GameObject.FindWithTag("Player").GetComponent<PlayerInput>().enabled = true;
    }
    #endregion

    #region Upgrade Menu
    public void LoadUpgradeScene()
    {
        originTimeScale = Time.timeScale;
        Time.timeScale = 0.05f; // This determines the animation's speed at PausedMenu. For example 0.05f := 1/20, thus the speed up is 20x.
        SceneManager.LoadScene("UpgradeMenu", LoadSceneMode.Additive);
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.GetComponent<PlayerInput>().enabled = false;
    }
    public void ExitUpgradeScene()
    {
        Time.timeScale = originTimeScale;
        this.LoadNextScene(); // TODO: determine next level
    }
    #endregion

    public void LoadTutorialLevelScene()
    {
        string s = "Scenes/Play/Tutorial";
        TransitionLoadScene(s);
    } 
    public void ExitLevelMenuScene()
    {
        string s = "Scenes/Menu/MainMenu";
        TransitionLoadScene(s);
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
        TransitionLoadScene(i);

    }
    public void LoadLevel(int int_index){
        int i = int_index + levelOffset;
        TransitionLoadScene(i);
    }

    public void LoadNextScene(){
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        TransitionLoadScene(i);

    }
    public void ReloadScene(){
        int i = SceneManager.GetActiveScene().buildIndex;
        TransitionLoadScene(i);
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
    
    void TransitionLoadScene(string s, float transitironDuration = 1.5f, float delay = 0.0f)
    {
        Transition.GetInstance().transitionDelay = delay;
        Transition.GetInstance().transitionDuration = transitironDuration;
        Transition.GetInstance().Exit(() => SceneManager.LoadScene(s));
    }
    void TransitionLoadScene(int i, float transitironDuration = 1.5f, float delay = 1.0f)
    {
        Transition.GetInstance().transitionDelay = delay;
        Transition.GetInstance().transitionDuration = transitironDuration;
        Transition.GetInstance().Exit(() => SceneManager.LoadScene(i));
    }
    #endregion
}

