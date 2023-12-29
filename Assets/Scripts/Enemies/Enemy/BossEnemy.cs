using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossEnemy : BaseEnemyBehavior
{
    public GameObject secondBulletPrefab;
    public GameObject thirdBulletPrefab;
    protected bool warpAttackOnCooldown;
    protected bool warpAttackDone;
    protected bool bullethellAttackOnCooldown;
    protected bool bullethellAttackDone;
    private Vector3 shootingPoint;
    [SerializeField] private EnemyHP bossHealthBar;
    [SerializeField] private TMP_Text bossName;
    protected override void Awake()
    {
        base.Awake();
        warpAttackOnCooldown = false;
        warpAttackDone = false;
        bullethellAttackOnCooldown = false;
        bullethellAttackDone = false;

        bossHealthBar = GetComponentInChildren<EnemyHP>();
        bossHealthBar.gameObject.SetActive(false);
        bossName = GetComponentInChildren<TMP_Text>();
        bossName.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        shootingPoint = transform.position + new Vector3(myFront.x*2, myFront.y*2, 0);
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, isPlayer);
        if (sleep)
        {
            if (playerInAttackRange)
            {
                StartCoroutine(WakeUp());
            }
        }
        else
        {
            if (currentHealth >= (maxHealth * 0.7f))
            {
                if (playerInAttackRange) AttackPlayer();
                else Chasing();
            }
            else if (((maxHealth * 0.7f) > currentHealth) && (currentHealth  >= (maxHealth * 0.3f)))
            {
                if (warpAttackOnCooldown && warpAttackDone)
                {
                    if (playerInAttackRange) AttackPlayer();
                    else Chasing();
                }
                else
                {
                    WarpAttack();
                }
            }
            
            else
            {
                if (bullethellAttackOnCooldown && bullethellAttackDone)
                {
                    if (warpAttackOnCooldown && warpAttackDone)
                    {
                        if (playerInAttackRange) AttackPlayer();
                        else Chasing();
                    }
                    else
                    {
                        WarpAttack();
                    }
                }
                else
                {
                    ShootBulletHell();
                }
            }
        }
    }

    protected override IEnumerator ShootRoutine()
    {
        Debug.Log("shoot Norm");
        yield return new WaitForSeconds(1f); //attack animation
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        if (bullet != null) 
        { 
            bullet.GetComponent<BaseBulletBehavior>().ShootAt(player);
            EnemyManager.GetInstance().EnemyShoot.Invoke(gameObject.transform.position, enemyName);
        }
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    protected IEnumerator ShootMulti(int bulletNum)
    {
        yield return new WaitForSeconds(3f);
    }

    protected IEnumerator Cooldown(float cooldown, int skill)
    {
        yield return new WaitForSeconds(cooldown);
        if(skill == 0)
        {
            warpAttackOnCooldown = false;
        }else if(skill == 1)
        {
            bullethellAttackOnCooldown = false;
        }
    }
    protected IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(3f);
        bossHealthBar.gameObject.SetActive(true);
        bossName.gameObject.SetActive(true);
        sleep = false;
    }

    private void WarpAttack()
    {
        if(warpAttackOnCooldown == false)
        {
            Debug.Log("Warp");
            warpAttackDone = false;
            StartCoroutine(Warping());
            warpAttackOnCooldown = true;
        }
        
    }

    protected IEnumerator Warping()
    {
        yield return new WaitForSeconds(2f); //wait warp animation
        transform.position = player.position + new Vector3(0, 0, 0);
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(2f); ///warp attack animation
        for(int i = 0; i<3; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            if (bullet != null)
            {
                bullet.GetComponent<BaseBulletBehavior>().ShootAt(player);
                EnemyManager.GetInstance().EnemyShoot.Invoke(gameObject.transform.position, enemyName);
            }
        }
        warpAttackDone = true;
        StartCoroutine(Cooldown(10f, 0));
    }

    private void ShootBulletHell()
    {
        if (bullethellAttackOnCooldown == false)
        {
            Debug.Log("Hell");
            bullethellAttackDone = false;
            agent.SetDestination(transform.position);
            StartCoroutine(SummonBulletHell());
            bullethellAttackOnCooldown = true;
        }
    }

    protected IEnumerator SummonBulletHell()
    {
        GameObject[] bulletSet = new GameObject[12];
        yield return new WaitForSeconds(2f); //wait casting animation
        for (int j = 0; j<3; j++)
        {
            for (int i = 0; i < 4; i++)
            {//instantiate bullet hell one by one
                bulletSet[i] = Instantiate(bulletPrefab, shootingPoint + new Vector3(j*0.2f,2.5f-i,0), Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }

            for (int i = 0; i < 4; i++)
            {
                //shoot all at once
                if (bulletSet[i] != null)
                {
                    bulletSet[i].GetComponent<BaseBulletBehavior>().ShootAt(player);
                    EnemyManager.GetInstance().EnemyShoot.Invoke(gameObject.transform.position, enemyName);
                }
            }
        }
        bullethellAttackDone = true;
        StartCoroutine(Cooldown(15f, 1));
    }

    //Temp
    public override void AdjustHealth(int deltaHealth)
    {
        base.AdjustHealth(deltaHealth);
        bossHealthBar.UpdateHealth(maxHealth, currentHealth);
    }
}
