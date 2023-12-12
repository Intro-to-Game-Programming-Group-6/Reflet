using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isPlayer;

    [Header("Prefabs")]
    [SerializeField] GameObject bulletPrefab;

    public float attackRange;
    public float attackDelay;

    
    [Header("Health values")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    public bool playerInAttackRange;
    bool isAttacking = false;

    List<GameObject> hearts = new List<GameObject>();
    Coroutine[] heartCoroutines;

    [Header("Heart Sprites")]
    [SerializeField] Sprite full;
    [SerializeField] Sprite empty;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        //this 2 line make navmesh work in 2d (must have in everything use navmesh)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, isPlayer);

        if (playerInAttackRange) AttackPlayer();
        else Chasing();
    }

    //just walk until reach attack range
    protected virtual void Chasing()
    {
        
        agent.isStopped = false;
        agent.SetDestination(player.position);

    }

    protected virtual void AttackPlayer()
    {
        agent.isStopped = true;
        //agent.SetDestination(transform.position);
        if (!isAttacking)
        {
            StartCoroutine(ShootRoutine());
            isAttacking = true;
        }
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, agent.transform.position, Quaternion.identity);
            bullet.GetComponent<BaseBulletBehavior>().ShootAt(player);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    public void AddHealth(int deltaHealth)
    {
        // print(deltaHealth);
        
        currentHealth += deltaHealth;

        if(hearts.Count == maxHealth)
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
        }
    }

    // private void Die()
    // {
    //     EnemyManager.GetInstance().HandleEnemyDeath();
    //     Destroy(this.gameObject);
    // }

    private void OnDestroy()
    {
        EnemyManager.GetInstance().HandleEnemyDeath();
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
