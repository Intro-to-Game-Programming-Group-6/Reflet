using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemyScript : MonoBehaviour
{
    private bool playerDetected;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isPlayer;

    [SerializeField] GameObject bulletPrefab;
    public float attackRange;
    public float attackDelay;

    public bool playerInAttackRange;
    bool isAttacking = false;

    public int health = 2;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        //this 2 line make navmesh work in 2d (must have in everything use navmesh)
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    //Basic state
    //Idle and Attack
    protected void Update()
    {
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, isPlayer);

        if (playerInAttackRange) AttackPlayer();
        else Idle();
    }

    protected virtual void Idle()
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
            GameObject bullet = Instantiate(bulletPrefab, agent.transform.position, Quaternion.identity, gameObject.transform);
            bullet.GetComponent<TestBulletScript>().ShootAt(player);
            yield return new WaitForSeconds(attackDelay);
        }
    }

    private void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

    public void AdjustHealth(int deltaHealth)
    {
        health += deltaHealth;

        if(health <= 0)
        {
            Vial.GetInstance().AddVialPoint(1);
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}