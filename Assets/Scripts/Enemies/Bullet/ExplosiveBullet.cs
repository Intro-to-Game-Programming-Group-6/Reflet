using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosiveBullet : BaseBulletBehavior
{
    //Explosive Bullet Behavior: only Not Reflectable bullet right now/Explode on contact with anything or when end of life time
    //is trigger
    //Checked
    //may add area of explosion in future
    public LayerMask isPlayer;
    public float explodingRange;
    [SerializeField] private int explosionCountdown = 3;

    bool playerInExplode;

    //Todo -> Change explosion timer to number of reflect

    /*
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Basic for all bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.GetInstance().AdjustHealth(-bulletDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (status == Status.OWNED_BY_PLAYER)
            {
                collision.gameObject.GetComponent<BaseEnemyBehavior>().AdjustHealth(-bulletDamage);
                Destroy(gameObject);
            }
            
        }
        else if (collision.gameObject.CompareTag("Reflector"))
        {
            //Make this also do damage
            PlayerManager.GetInstance().AdjustHealth(-bulletDamage);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall") | collision.gameObject.CompareTag("Obstacles"))
        {
            Destroy(this.gameObject);
        }
    }
    */
    

    [SerializeField] protected Collider2D col;
    protected override void OnEnable()
    {
        base.OnEnable();
        col = GetComponent<Collider2D>();

        //col.enabled = false; 
        col.isTrigger = true;//prevent it to crash with enemy collider
    }
    //special case of our only non-trigger type of bullet
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (status == Status.OWNED_BY_ENEMY)
            {
                Explode();
                //PlayerManager.GetInstance().AdjustHealth(-bulletDamage);
                //Destroy(this.gameObject);
            }
            else
            {
                Vector2 inNorm = collision.contacts[0].normal;
                //ReflectBullet
                ReflectBullet(inNorm);
            }

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (explosionCountdown > 0)
            {
                Vector2 inNorm = collision.contacts[0].normal;
                //ReflectBullet
                status = Status.OWNED_BY_ENEMY;
                explosionCountdown -= 1;
                ReflectBullet(inNorm);
            }
            else
            {
                Explode();
            }
                
            
        }
        //All Reflectable should have this
        else if (collision.gameObject.CompareTag("Reflector"))
        {
            if(explosionCountdown > 0)
            {
                Vector2 inNorm = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - GameObject.Find("Player").transform.position;

                ReflectBullet(inNorm);
                explosionCountdown -= 1;
                status = Status.OWNED_BY_PLAYER; //allow bullet to hit enemy maybe reverse back to owned by enemy when we add enemy that can also reflect bullet in the future
            }
            else
            {
                Explode();
            }
            
        }
        else if (collision.gameObject.CompareTag("Wall") | collision.gameObject.CompareTag("Obstacles"))
        {
            if (explosionCountdown > 0)
            {
                //Debug.Log("Hit " + collision.gameObject.tag);
                //Get direction to bounce
                Vector2 inNorm = collision.contacts[0].normal;
                //ReflectBullet
                explosionCountdown -= 1;
                status = Status.OWNED_BY_ENEMY;
                ReflectBullet(inNorm);
            }
            else
            {
                Explode();
            }
                
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            if (explosionCountdown > 0)
            {
                //uncommend this if choose bouncing to not collapse with other bouncing
                //col.isTrigger = true;
                //uncommend this to make bouncing bounce each other
                Vector2 inNorm = collision.contacts[0].normal;
                //ReflectBullet
                explosionCountdown -= 1;
                status = Status.OWNED_BY_ENEMY;
                ReflectBullet(inNorm);
            }
            else
            {
                Explode();
            }   
            
        }


    }

    protected override void Update()
    {
        //base.Update();

        playerInExplode = Physics2D.OverlapCircle(transform.position + new Vector3(0.3f, 0f, 0f), explodingRange, isPlayer);
    }
    protected override void EndLifetime()
    {
        if (lifetimeCount >= bulletLifeTime)
        {
            Explode();
            //Destroy(this.gameObject);
        }
    }


    Collider2D[] objInExplode = new Collider2D[10];
    private void Explode()
    {
        Debug.Log("Exploded");
        //add explosion effect here
        if (playerInExplode)
        {
            PlayerManager.GetInstance().AdjustHealth(-bulletDamage);

        }

        Physics2D.OverlapCircle(transform.position + new Vector3(0.3f, 0f, 0f), explodingRange, new ContactFilter2D(), objInExplode);
        if (objInExplode.Length == 0 || objInExplode == null) {
            foreach (Collider2D en in objInExplode)
            {
                if (en.gameObject.tag == "Enemy")
                {
                    en.gameObject.GetComponent<BaseEnemyBehavior>().AdjustHealth(-bulletDamage);
                }

            }
        }
        Destroy(this.gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //do nothing since this is only non-trigger bullet
        //this isn't repeated code this make sur reflector work either it is triiger or not
        if (collision.gameObject.CompareTag("Reflector"))
        {
            if (explosionCountdown > 0)
            {
                Vector2 inNorm = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - GameObject.Find("Player").transform.position;

                ReflectBullet(inNorm);
                explosionCountdown -= 1;
                status = Status.OWNED_BY_PLAYER; //allow bullet to hit enemy maybe reverse back to owned by enemy when we add enemy that can also reflect bullet in the future
            }
            else
            {
                Explode();
            }

        }
    }

    //make it act normally after leaving enemy
    //another fix: make the bullet appear outside of the enemy collider
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            col.isTrigger = false;
        }
        else if (collider.CompareTag("Bullet"))
        {
            //uncommend this if choose bouncing to not collapse with other bouncing
            //col.isTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0.3f,0f,0f), explodingRange);
        }
    }


}
