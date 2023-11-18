using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    private static Notification instance;

    private SpriteRenderer notifSprite;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Notification GetInstance()
    {
        return instance;
    }

    void Start()
    {
        notifSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(PlayerControlScript.GetInstance().GetInteractions())
        {
            notifSprite.enabled = true;
        }
        else
        {
            notifSprite.enabled = false;
        }
    }
}
