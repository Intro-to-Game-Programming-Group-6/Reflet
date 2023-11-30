using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class In_Game_UI : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas InGameUI;
    public Canvas PauseMenu;
    private float origin_timescale;

    void Start()
    {
        origin_timescale = Time.timeScale;
        PauseMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        Debug.Log("pressed to pause");
        InGameUI.enabled = false;
        PauseMenu.enabled = true;
        Time.timeScale = 0f;
    }

    public void ReturnToGame()
    {
        InGameUI.enabled = true;
        PauseMenu.enabled = false;
        Time.timeScale = origin_timescale;
    }
}
