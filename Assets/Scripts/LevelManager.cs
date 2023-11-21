using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public void LoadOptionScene(){
        SceneManager.LoadScene("OptionMenu", LoadSceneMode.Additive);
    }
    public void ExitOptionScene(){
        // Gameplay gp = FindObjectOfType<Gameplay>();
        // gp._paused = false;
        SceneManager.UnloadSceneAsync("OptionMenu");
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

    public void QuitGame(){
        Application.Quit();
    }

    public void UnloadScene(string string_name){
        SceneManager.UnloadSceneAsync(string_name);
    }
}

