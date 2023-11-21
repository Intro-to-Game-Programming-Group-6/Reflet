using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionSettings : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider VolumeSlider;
    void Start()
    {
        VolumeSlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MuteVolume()
    {
        //NEED TO IMPLEMENT SOUND MANAGER
        //TOGGLE MUTE HERE
        //    MusicManager.GetInstance().ToggleMute();
        //
    }

    public void ChangeVolume(float newvolume)
    {
        //NEED TO IMPLEMENT SOUND MANAGER
        //TOGGLE MUTE HERE
        //    MusicManager.GetInstance().SetVolume(newvolume);
        //
    }
}
