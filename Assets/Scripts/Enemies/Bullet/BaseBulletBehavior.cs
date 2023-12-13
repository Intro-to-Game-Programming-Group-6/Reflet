using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseBulletBehavior : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;

    public float bulletSpeed;
    public float bulletLifeTime;
    protected float lifetimeCount;
    public float bulletDamage;
    public float knockbackForce = 2f;

    public enum Status
    {
        OWNED_BY_PLAYER,
        OWNED_BY_ENEMY
    }
    public Status status;

    protected virtual void OnEnable()
    {
        status = Status.OWNED_BY_ENEMY;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        lifetimeCount = 0;
    }

    private void Update()
    {
        lifetimeCount += 0.001f; // make lifetime close to second
        if (lifetimeCount >= bulletLifeTime) EndLifetime();
    }
    public virtual void ShootAt(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
        //transform.LookAt(target); this look 'into' the screen
        //Use this instance of lookat
        transform.right = direction; //work now but don't know why
    }

    protected virtual void EndLifetime()
    {
        //make it disappear after sometime
        if (lifetimeCount >= bulletLifeTime)
        {
            Destroy(this.gameObject);
        }

    }

    protected Vector2 lastvelocity;
    private void FixedUpdate()
    {
        //velocity of bullet before unity apply any physics operation on it
        lastvelocity = rb.velocity;
    }

    public virtual void ReflectBullet(Vector2 inNorm)
    {
        Vector2 re_dir = Vector2.Reflect(lastvelocity, inNorm).normalized;
        rb.velocity = re_dir * bulletSpeed;
        //reset the direction of bullet
        transform.right =  re_dir;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Basic for all bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            if (status == Status.OWNED_BY_ENEMY)
            {
                PlayerManager.GetInstance().AdjustHealth(-1);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (status == Status.OWNED_BY_PLAYER)
            {
                collision.gameObject.GetComponent<BaseEnemyBehavior>().AdjustHealth(-1);
                collision.gameObject.GetComponent<BaseEnemyBehavior>().Knockback(gameObject.transform, knockbackForce);
                Destroy(gameObject);
            }
            
        }
        //All Reflectable should have this
        else if (collision.gameObject.CompareTag("Reflector"))
        {
            Vector2 inNorm = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - GameObject.Find("Player").transform.position;

            ReflectBullet(inNorm);

            status = Status.OWNED_BY_PLAYER; //allow bullet to hit enemy maybe reverse back to owned by enemy when we add enemy that can also reflect bullet in the future
        }
    }
}
