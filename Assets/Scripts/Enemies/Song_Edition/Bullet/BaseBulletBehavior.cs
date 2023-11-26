using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletBehavior : MonoBehaviour
{
    //public GameObject selfPrefab;
    public float bulletSpeed;
    public float bulletLifeTime;
    private float lifetimeCount;
    public float bulletDamage;
    [SerializeField] protected Rigidbody2D rb;

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
        Vector2 direction = target.position - transform.position;
        rb.velocity = direction.normalized * bulletSpeed;
        //transform.LookAt(target);
        //Use this instance of lookat
        //transform.right = target.position - transform.position;
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
        //Vector3 re_dir3d = re_dir;
        //transform.LookAt(re_dir);
        //transform.right =  re_dir3d;
    }
}
