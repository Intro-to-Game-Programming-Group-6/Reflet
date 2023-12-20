using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[CreateAssetMenu]
public class Dash : BaseAbillity
{
    Vector2 dashDirection;

    public IEnumerator GoDash(PlayerControlScript control)
    {
        if(control.GetRigidBody().velocity.magnitude < 0.1f) yield break;

        control.canDash = false;
        control.currentlyDashing = true;

        dashDirection = control.GetComponent<Rigidbody2D>().velocity.normalized;
        control.GetRigidBody().velocity = dashDirection * control.dashSpeed;
        
        yield return new WaitForSeconds(control.dashDuration);
        control.currentlyDashing = false;
        yield return new WaitForSeconds(control.dashCooldown);
        control.canDash = true;
    }
}
