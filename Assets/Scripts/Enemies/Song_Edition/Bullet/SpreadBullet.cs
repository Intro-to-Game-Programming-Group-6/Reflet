using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : BaseBulletBehavior
{
    // Spread Bullet Behavior: Reflectable/detroy at end of life time/ spread into smaller bullet(other bullet variation) when getting reflected or pass some time?
    //is trigger
    public GameObject spreadingBulletPrefeb;
    protected override void EndLifetime()
    {
        //get current direction & speed
        Vector2 cur_dir = rb.velocity.normalized;
        float cur_speed = rb.velocity.magnitude;

        base.EndLifetime();
    }
}
