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

        if (PlayerManager.GetInstance() != null)
        {
            Destroy(PlayerManager.GetInstance().gameObject);
        }
    }

    public static MainMenu GetInstance()
    {
        return instance;
    }
    public void MoveToGameplay()
    {
        SceneManager.LoadScene("Test Bullet Reflect");
    }

    public void MoveToSettings() {
        SceneManager.LoadScene("OptionMenu");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
