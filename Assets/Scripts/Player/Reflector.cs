using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Reflector : MonoBehaviour
{
    //private static Reflector instance;

    public float duration = 1.0f;
    private float timer = 0.0f;

    private bool active;

    void Awake()
    {
        /*
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }

    /*
    public static Reflector GetInstance()
    {
        return instance;
    }
    */

    void OnEnable()
    {
        active = true;
        
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), PlayerControlScript.GetInstance().GetComponent<Collider2D>(), true);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collision happened!");
        Debug.Log(collision.gameObject.tag);
    }

    void Update()
    {
        /*
        if (!active) {
            Destroy(gameObject);
            return;
        }

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            active = false;
        }
        */
    }
}
