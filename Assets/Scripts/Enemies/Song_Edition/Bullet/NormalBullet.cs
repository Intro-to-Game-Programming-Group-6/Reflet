using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBulletBehavior
{
    //Normal Bullet Behavior: Reflectable/detroy when hit wall or at end of life time
    //is trigger
    //Checked
    //Detroy on wall contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {
            //PlayerManager.GetInstance().AdjustHealth(-1);
            Destroy(gameObject);
        }
        else if(collision.tag == "Wall" | collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (status == Status.OWNED_BY_PLAYER)
            {
                collision.gameObject.GetComponent<BaseEnemyBehavior>().AdjustHealth(-1);
            }
            Destroy(gameObject);
        }
    }

}
