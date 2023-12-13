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
        control.canDash = false;
        control.currentlyDashing = true;
        dashDirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - control.GetRigidBody().transform.position;
        dashDirection = dashDirection.normalized;
        control.GetRigidBody().velocity = dashDirection * control.dashSpeed;
        yield return new WaitForSeconds(control.dashDuration);
        control.currentlyDashing = false;
        yield return new WaitForSeconds(control.dashCooldown);
        control.canDash = true;
    }
}
