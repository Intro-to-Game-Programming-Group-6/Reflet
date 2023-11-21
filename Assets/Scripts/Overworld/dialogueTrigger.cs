using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : MonoBehaviour
{
    public TextAsset dialogue;
    public bool canPlay;

    public GameObject exclamation;
    private bool playerDetected;

    private void Start()
    {
        playerDetected = false;
        exclamation = gameObject.transform.GetChild(0).gameObject;

        exclamation.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerDetected = true;
            exclamation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            exclamation.SetActive(false);
        }
    }
}
