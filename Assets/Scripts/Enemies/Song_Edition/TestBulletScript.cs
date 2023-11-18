using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletScript : MonoBehaviour
{
    public GameObject selfPrefab;
    public float bulletSpeed;
    public float bulletLifeTime;
    private float lifetimeCount;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        lifetimeCount = 0;
    }

    private void Update()
    {
        lifetimeCount += 0.001f;
        if (lifetimeCount >= bulletLifeTime) EndLifetime();
    }
    public void ShootAt(Transform player)
    {
        Vector2 direction = player.position - transform.position;
        //GetComponent<Rigidbody2D>().AddForce(direction.normalized * bulletSpeed, ForceMode2D.Force);
        rb.velocity = direction.normalized * bulletSpeed;
    }

    private void EndLifetime()
    {
        //make it disappear after sometime
        //double check
        if (lifetimeCount >= bulletLifeTime)
        {
            Destroy(this.gameObject);
        }

    }

    public void ReflectBullet(Vector2 inNorm)
    {
        Vector2 re_dir = Vector2.Reflect(rb.velocity, inNorm).normalized;
        rb.velocity = -re_dir * bulletSpeed;
    }

    //Test boucing with wall
    Vector2 lastvelocity;
    private void FixedUpdate()
    {
        lastvelocity = rb.velocity;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit Wall!");
        //Vector2 inVelo = rb.velocity;
        //rb.velocity = Vector2.zero;
        Vector2 inNorm = collision.contacts[0].normal;
        //ReflectBullet(inVelo, inNorm);
        Vector2 re_dir = Vector2.Reflect(lastvelocity, inNorm).normalized;

        rb.velocity = re_dir * bulletSpeed;

    }
    
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit!");
        ContactPoint2D[] contacts = new ContactPoint2D[2];

        collision.GetContacts(contacts);

        Vector2 inNorm = contacts[0].normal;

        ReflectBullet(inNorm);
    }
    */
}