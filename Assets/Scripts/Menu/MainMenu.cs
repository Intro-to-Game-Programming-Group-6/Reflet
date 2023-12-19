using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        if (PlayerManager.GetInstance() != null)
        {
            Destroy(PlayerManager.GetInstance().gameObject);
        }

        if (PlayerControlScript.GetInstance() != null)
        {
            Destroy(PlayerControlScript.GetInstance().gameObject);
        }
    }

    public static MainMenu GetInstance()
    {
        return instance;
    }
    
    public void LoadLevel(int level)
    {
        LevelManager.GetInstance().LoadScene(level);
    }

    public void LoadTutorialLevelScene()
    {
        LevelManager.GetInstance().LoadTutorialLevelScene();
    }

    public void LoadOptionScene()
    {
        LevelManager.GetInstance().LoadOptionScene();
    }

    public void QuitGame()
    {
        LevelManager.GetInstance().QuitGame();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
