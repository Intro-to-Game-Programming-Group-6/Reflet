using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[CreateAssetMenu]
public class Dash : BaseAbillity
{
    Vector2 dashDirection;

    public IEnumerator GoDash(PlayerControlScript control, DashManager manager)
    {
        if (control.GetRigidBody().velocity.magnitude < 0.1f || manager.dashAvailability <= 0) yield break;

        manager.canDash = false;
        manager.isDashing = true;

        // Dash
        dashDirection = control.GetComponent<Rigidbody2D>().velocity.normalized;
        control.GetRigidBody().velocity = dashDirection * manager.dashSpeed;
        
        yield return new WaitForSeconds(manager.dashDuration);
        manager.isDashing = false;

        if (manager.dashAvailability > 0)
        {
            manager.dashAvailability--;
            manager.canDash = true;
            yield break;
        }
    }
}
