using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    private static LevelManager Instance;
    private static float originTimeScale;
    
    [SerializeField] private int currentSceneIndex;
    [SerializeField] private int selectedLevel;
    [SerializeField] private int levelOffset;

    void Awake(){
        if (Instance == null)
        {
            Instance = this;
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

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
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

    public void LoadLevelMenuScene()
    {
        SceneManager.LoadScene("LevelMenu");
    } 

    public void ExitLevelMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadScene(string string_name)
    {
        SceneManager.LoadScene(string_name);
    }

    public void LoadScene(int int_index)
    {
        SceneManager.LoadScene(int_index);
    }

    public void LoadLevel(int int_index)
    {
        SceneManager.LoadScene(int_index + levelOffset);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}

