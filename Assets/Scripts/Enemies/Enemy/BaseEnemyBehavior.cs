using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BaseEnemyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject bulletPrefab;
    public bool sleep;
    

    [Header("Animation Properties")]
    [HideInInspector][SerializeField] Animator animController;
    [HideInInspector][SerializeField] private SpriteRenderer sprite;
    [HideInInspector][SerializeField] static int AnimatorWalk = Animator.StringToHash("Walk");
    [HideInInspector][SerializeField] static int AnimatorAttack = Animator.StringToHash("Attack");

    [Header("Player Variables")]
    [SerializeField] protected Transform player;
    [SerializeField] protected LayerMask isPlayer;

    [Header("Attack Variables")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float detectRange;
    [HideInInspector] [SerializeField] protected bool playerInAttackRange;
    [HideInInspector] [SerializeField] protected bool playerInDetectRange;
    [HideInInspector] [SerializeField] protected bool isAttacking = false;

    [Header("Health Components")]
    [SerializeField] Sprite full;
    [SerializeField] Sprite empty;
    [HideInInspector][SerializeField] List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private int maxHealth;
    [HideInInspector][SerializeField] private int currentHealth;
    [HideInInspector][SerializeField] Coroutine[] heartCoroutines;

    [Header("Effects")]
    [SerializeField] protected GameObject hurtEffect;
    [SerializeField] protected GameObject dieEffect;
    [SerializeField] protected GameObject shootEffect;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animController = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        //this 2 line make navmesh work in 2d (must have in everything use navmesh)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        sleep = true;
        currentHealth = maxHealth;

    }

    protected virtual void OnEnable()
    {
        heartCoroutines = new Coroutine[maxHealth];

        foreach (Transform heart in transform)
        {
            hearts.Add(heart.gameObject);
            heart.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }

    //Basic enemy only walk to player and attack
    protected virtual void Update()
    {
        if (sleep)
        {
            StartCoroutine(Dizzy());
        }
        else
        {
            playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, isPlayer);
            // playerInDetectRange = Physics2D.OverlapCircle(transform.position, detectRange, isPlayer);
            
            Chasing();

            if (playerInAttackRange) AttackPlayer();
            // else if (playerInDetectRange && !playerInAttackRange) Chasing();
            // else Idle();
        }
        
    }

    private void FixedUpdate()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        sprite.flipX = direction.x > 0f;
    }

    protected virtual void Idle()
    {
        //animController.SetBool(AnimatorWalk, false);
        agent.isStopped = true;
        agent.SetDestination(transform.position);
    }

    //just walk until reach attack range
    protected virtual void Chasing()
    {
        //animController.SetBool(AnimatorWalk, true);
        agent.isStopped = false;
        agent.SetDestination(player.position);

    }

    protected IEnumerator Dizzy()
    {
        yield return new WaitForSeconds(2f);
        sleep = false;
    }

    public void Knockback(Transform collision, float knockbackForce)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Vector2 knockbackDirection = (transform.position - collision.position).normalized;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 currentVelocity = rb.velocity;
                Vector2 knockbackVelocity = knockbackDirection * knockbackForce;
                Vector2 newVelocity = new Vector2(
                    Mathf.Abs(knockbackVelocity.x) > Mathf.Abs(currentVelocity.x) ? 0 : currentVelocity.x,
                    Mathf.Abs(knockbackVelocity.y) > Mathf.Abs(currentVelocity.y) ? 0 : currentVelocity.y
                );

                rb.velocity = newVelocity;

                rb.AddForce(knockbackVelocity, ForceMode2D.Impulse);
            }
        }
    }

    protected virtual void AttackPlayer()
    {
        //agent.isStopped = true;
        
        
        if (!isAttacking)
        {
            agent.SetDestination(transform.position);
            StartCoroutine(ShootRoutine());
            isAttacking = true;
        }
    }

    protected virtual IEnumerator ShootRoutine()
    {
        while (true)
        {
            //Instantiate(shootEffect, transform.position, Quaternion.identity);
            EnemyManager.GetInstance().EnemyShoot.Invoke(gameObject.transform.position);
            GameObject bullet = Instantiate(bulletPrefab, agent.transform.position, Quaternion.identity);
            bullet.GetComponent<BaseBulletBehavior>().ShootAt(player);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    public void AdjustHealth(int deltaHealth)
    {
        currentHealth += deltaHealth;

        UpdateHearts();
        //Instantiate(currentHealth <= 0 ? dieEffect: hurtEffect, transform.position, Quaternion.identity);
        
        if (currentHealth <= 0)
        {
            EnemyManager.GetInstance().EnemyDie.Invoke(transform.position);
            PlayerManager.GetInstance().AddVialPoint(1);
            Destroy(this.gameObject);
        }
        else {
            EnemyManager.GetInstance().EnemyShoot.Invoke(gameObject.transform.position);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            /*
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);
            */
        }
    }

    private void OnDestroy()
    {
        EnemyManager.GetInstance().HandleEnemyDeath(transform.position);
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < maxHealth; i ++)
        {
            if (heartCoroutines[i] != null)
            {
                StopCoroutine(heartCoroutines[i]);
            }

            heartCoroutines[i] = StartCoroutine(ShowHeart(hearts[i], i));

            if(i < currentHealth)
            {
                hearts[i].GetComponent<SpriteRenderer>().sprite = full;
            }
            else
            {
                hearts[i].GetComponent<SpriteRenderer>().sprite = empty;
            }
        }
    }

    IEnumerator ShowHeart(GameObject heart, int idx)
    {
        heart.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(1f);
        heart.GetComponent<SpriteRenderer>().enabled = false;
        heartCoroutines[idx] = null;
    }
}
