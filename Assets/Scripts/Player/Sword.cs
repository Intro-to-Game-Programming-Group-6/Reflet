using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    private static Sword instance;

    public Transform pivotPoint;
    public float rotationSpeed;

    public float duration = 1.0f;
    private float timer = 0.0f;

    private bool active;

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

    public static Sword GetInstance()
    {
        return instance;
    }

    void OnEnable()
    {
        active = true;
    }

    void Update()
    {
        if (!active) {
            Destroy(gameObject);
            return;
        }

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            active = false;
        }
    }

    //make sword reflect bullet
    //[SerializeField] private Rigidbody2D playerRb;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D bulletRb;
        if (collision.tag == "Bullet")
        {
            Debug.Log("Reflect!");
            bulletRb = collision.GetComponent<Rigidbody2D>();
            Vector2 bulletInVelocity = bulletRb.velocity;
            float bulletspeed = bulletInVelocity.magnitude;
            Vector2 clickdirection = (CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;
            //Vector2 re_dir = Vector2.Reflect(bulletInVelocity, clickdirection);
            //test reflect same way
            Vector2 re_dir = Vector2.Reflect(bulletInVelocity, bulletInVelocity.normalized);
            bulletRb.velocity = re_dir*bulletspeed;
        }
        //get inNorm
        //Vector2 clickdirection = (CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;
        //get inVelocity from bullet

        //Call Reflect from bullet

    }
}
