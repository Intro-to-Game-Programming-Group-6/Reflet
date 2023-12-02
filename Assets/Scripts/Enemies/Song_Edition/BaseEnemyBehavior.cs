using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isPlayer;

    [SerializeField] GameObject bulletPrefab;
    public float attackRange;
    public float attackDelay;
    [SerializeField] private int HP;

    public bool playerInAttackRange;
    bool isAttacking = false;


    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        //this 2 line make navmesh work in 2d (must have in everything use navmesh)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    //Basic enemy only walk to player and attack
    protected virtual void Update()
    {
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, isPlayer);

        if (playerInAttackRange) AttackPlayer();
        else Chasing();
    }

    protected virtual void Chasing()
    {
        //just walk until reach attack range
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

    public void AdjustHealth(int deltaHealth)
    {
        HP += deltaHealth;

        if (HP <= 0)
        {
            //Vial.GetInstance().AddVialPoint(1);
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }


}
