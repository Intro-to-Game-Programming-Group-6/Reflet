using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private static float originTimeScale;

    void Awake(){
        if (instance == null){
            instance = this;
            originTimeScale = Time.timeScale;
            Debug.Log("awake"+Time.timeScale);

            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

    void Start(){
        Debug.Log("start"+Time.timeScale);
    }

    void Update(){
        // if (Input.GetKeyDown(KeyCode.Escape)){
        //     if (!SceneManager.GetSceneByName("PausedMenu").isLoaded){
        //         LoadPausedScene();
        //     }
        // }
    }
    // public GameManager GM;
    public void LoadOptionScene(){
        SceneManager.LoadScene("OptionMenu", LoadSceneMode.Additive);
    }
    public void ExitOptionScene(){
        SceneManager.UnloadSceneAsync("OptionMenu");
    } 

    public void LoadPausedScene(){
        SceneManager.LoadScene("PausedMenu", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
    public void ExitPausedScene(){
        SceneManager.UnloadSceneAsync("PausedMenu");
        Debug.Log(originTimeScale);
        Time.timeScale = originTimeScale;
    } 


    public void LoadScene(string string_name){
        SceneManager.LoadScene(string_name);
    }
    public void LoadScene(int int_index){
        SceneManager.LoadScene(int_index);
    }

    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame(){
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame(){
        Application.Quit();
    }

    public void UnloadScene(string string_name){
        SceneManager.UnloadSceneAsync(string_name);
    }
}

