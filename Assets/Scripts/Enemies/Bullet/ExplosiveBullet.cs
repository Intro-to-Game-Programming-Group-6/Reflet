using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : BaseBulletBehavior
{
    //Explosive Bullet Behavior: only Not Reflectable bullet right now/Explode on contact with anything or when end of life time
    //is trigger
    //Checked
    //may add area of explosion in future
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


}
