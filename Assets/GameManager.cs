using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private SoundManager SM;
    private LevelManager LM;
    public static GameManager instance;
    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
            return;
        }

        // Get Managers
        LM = GetComponent<LevelManager>();
        SM = GetComponent<SoundManager>();

        // Set for Sound Manager Player Prefs
        PlayerPrefs.SetFloat("CurrVolume", 0.5f);
        PlayerPrefs.SetFloat("TempVolume", 0.5f);
        PlayerPrefs.SetFloat("Mute", 0f);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(PlayerPrefs.GetFloat("CurrVolume"));
        // Debug.Log((PlayerPrefs.GetFloat("CurrVolume"))>0.0f);
    }
}
