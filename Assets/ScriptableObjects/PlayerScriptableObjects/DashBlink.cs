using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[CreateAssetMenu]
public class DashBlink : BaseAbillity
{
    Vector2 dashDirection;
    public IEnumerator GoDash(PlayerControlScript control)
    {
        if(control.GetRigidBody().velocity.magnitude < 0.1f) yield break;

        control.canDash = false;
        control.currentlyDashing = true;

        dashDirection = control.GetComponent<Rigidbody2D>().velocity.normalized;
        float max_range = control.dashSpeed * control.dashDuration;
        max_range = Mathf.Min(max_range, dashDirection.magnitude);

        control.GetRigidBody().MovePosition(control.GetRigidBody().position + dashDirection * max_range);
        yield return new WaitForSeconds(control.dashDuration);
        control.currentlyDashing = false;
        yield return new WaitForSeconds(control.dashCooldown);
        control.canDash = true;
    }
}
