using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletScript : MonoBehaviour
{
    public GameObject selfPrefab;

    public enum Status {
        FRESHLY_CREATED,
        REFLECTED_BY_PLAYER,
        REFLECTED_NOT_BY_PLAYER
    }
    
    public Status status;
    public float bulletSpeed;
    public float bulletLifeTime;
    private Vector2 lastvelocity;

    private Rigidbody2D rb;
    private Collider2D col;

    private void OnEnable()
    {
        status = Status.FRESHLY_CREATED;

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        col.enabled = false;
    }

    private void Start() {
        Destroy(gameObject, bulletLifeTime);
    }

    public void ShootAt(Transform player)
    {
        Vector2 direction = player.position - transform.position;
        //GetComponent<Rigidbody2D>().AddForce(direction.normalized * bulletSpeed, ForceMode2D.Force);
        rb.velocity = direction.normalized * bulletSpeed;
    }

    //Test boucing with wall

    private void FixedUpdate()
    {
        lastvelocity = rb.velocity;
    }
    
    //Make bullet Bounce off the wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacles"))
        {
            //Vector2 inVelo = rb.velocity;
            //rb.velocity = Vector2.zero;
            Vector2 inNorm = collision.contacts[0].normal;
            //ReflectBullet(inVelo, inNorm);
            Vector2 re_dir = Vector2.Reflect(lastvelocity, inNorm).normalized;
            status = Status.REFLECTED_NOT_BY_PLAYER;
            rb.velocity = re_dir * bulletSpeed;
        }
        else if(collision.gameObject.CompareTag("Reflector"))
        {
            //Vector2 inVelo = rb.velocity;
            //rb.velocity = Vector2.zero;
            Vector2 inNorm = collision.contacts[0].normal;
            //ReflectBullet(inVelo, inNorm);
            Vector2 re_dir = Vector2.Reflect(lastvelocity, inNorm).normalized;

            rb.velocity = re_dir * bulletSpeed;
            
            status = Status.REFLECTED_BY_PLAYER;
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.GetInstance().AdjustHealth(-1);
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            if(status == Status.REFLECTED_BY_PLAYER) {
                collision.gameObject.GetComponent<TestEnemyScript>().AdjustHealth(-1);
            }
            Destroy(gameObject);
        }
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            col.enabled = true;
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit!");
        ContactPoint2D[] contacts = new ContactPoint2D[2];

        collision.GetContacts(contacts);

        Vector2 inNorm = contacts[0].normal;

        Vector2 re_dir = Vector2.Reflect(lastvelocity, inNorm).normalized;

        rb.velocity = re_dir * bulletSpeed;
    }
    */
}