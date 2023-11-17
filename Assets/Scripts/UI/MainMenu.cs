using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToGameplay()
    {
        SceneManager.LoadScene("TestingBobby");
    }

    public void MoveToTutorial()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
