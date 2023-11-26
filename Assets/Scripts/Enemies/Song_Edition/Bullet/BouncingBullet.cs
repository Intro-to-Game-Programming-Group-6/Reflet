using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : BaseBulletBehavior
{
    //Bouncing Bullet Behavior: Reflectable/bouncing again wall and obstacle/destroy when end of life time
    //not trigger
    //Checked
    //Bouncing against wall and obstacle
    [SerializeField] protected Collider2D col;
    protected override void OnEnable()
    {
        base.OnEnable();
        col = GetComponent<Collider2D>();

        col.enabled = false; //prevent it to crash with enemy collider
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Wall" | collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("Hit " + collision.gameObject.tag);
            //Get direction to bounce
            Vector2 inNorm = collision.contacts[0].normal;
            //ReflectBullet
            ReflectBullet(inNorm);
        }


    }

    //make it act normally after leaving enemy
    //another fix: make the bullet appear outside of the enemy collider
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            col.enabled = true;
        }
    }
}
