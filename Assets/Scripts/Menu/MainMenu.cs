using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void MoveToGameplay()
    {
        // SceneManager.LoadScene("MapOswinTest");
    }

    public void MoveToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
