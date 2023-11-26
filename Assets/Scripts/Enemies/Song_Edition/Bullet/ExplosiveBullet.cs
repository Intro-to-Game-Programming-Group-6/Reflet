using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : BaseBulletBehavior
{
    //Explosive Bullet Behavior: Not Reflectable/Explode on contact with anything or when end of life time
    //is trigger
    //Checked
    public override void ReflectBullet(Vector2 inNorm)
    {
        //DoDamage with get reflect
        Destroy(this.gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            //Do Damage
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Wall" | collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }


}
