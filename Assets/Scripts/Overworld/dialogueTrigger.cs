using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : MonoBehaviour
{
    public TextAsset dialogue;
    public bool canPlay;

    private bool playerDetected;

    private void Start()
    {
        playerDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerDetected = true;
            PlayerControlScript.GetInstance().ToggleInteractions(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            PlayerControlScript.GetInstance().ToggleInteractions(false);
        }
    }
}
