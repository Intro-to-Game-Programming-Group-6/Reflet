using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyBehavior : MonoBehaviour
{
    private SpriteRenderer sprite;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isPlayer;

    [Header("Prefabs")]
    [SerializeField] GameObject bulletPrefab;

    public float detectRange;
    public float attackRange;
    public float attackDelay;

    
    [Header("Health values")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private bool sleep;
    public bool playerInAttackRange;
    public bool playerInDetectRange;
    bool isAttacking = false;

    static int AnimatorWalk = Animator.StringToHash("Walk");
    static int AnimatorAttack = Animator.StringToHash("Attack");
    Animator animController;

    List<GameObject> hearts = new List<GameObject>();
    Coroutine[] heartCoroutines;

    [Header("Heart Sprites")]
    [SerializeField] Sprite full;
    [SerializeField] Sprite empty;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animController = GetComponent<Animator>();
        //this 2 line make navmesh work in 2d (must have in everything use navmesh)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        sleep = true;

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
            playerInDetectRange = Physics2D.OverlapCircle(transform.position, detectRange, isPlayer);

            if (playerInAttackRange && playerInAttackRange) AttackPlayer();
            else if (playerInDetectRange && !playerInAttackRange) Chasing();
            else Idle();
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

    protected virtual void AttackPlayer()
    {
        //animController.SetBool(AnimatorWalk, false);
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        if (!isAttacking)
        {
            StartCoroutine(ShootRoutine());
            isAttacking = true;
        }
    }

    IEnumerator Dizzy()
    {
        yield return new WaitForSeconds(5f);
        sleep = false;
    }

    IEnumerator ShootRoutine()
    {
            //animController.SetTrigger(AnimatorAttack);
            GameObject bullet = Instantiate(bulletPrefab, agent.transform.position, Quaternion.identity);
            bullet.GetComponent<BaseBulletBehavior>().ShootAt(player);
            yield return new WaitForSeconds(attackDelay);
            isAttacking = false;
    }



    public void AdjustHealth(int deltaHealth)
    {
        // print(deltaHealth);

        currentHealth += deltaHealth;

        if (hearts.Count == maxHealth)
        {
            UpdateHearts();
        }

        if (currentHealth <= 0)
        {
            PlayerManager.GetInstance().AddVialPoint(1);
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }

    //private void Die()
    //{
    //    EnemyManager.GetInstance().HandleEnemyDeath();
    //    Destroy(this.gameObject);
    //}

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
