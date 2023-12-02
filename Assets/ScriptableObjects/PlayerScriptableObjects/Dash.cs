using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[CreateAssetMenu]
public class Dash : BaseAbillity
{
    Vector2 dashDirection;
    float dashSpeed = 6f;

    public IEnumerator GoDash(PlayerControlScript control)
    {
        control.canDash = false;
        control.currentlyDashing = true;
        dashDirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - control.GetRigidBody().transform.position;
        dashDirection = dashDirection.normalized;
        control.GetRigidBody().velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(0.5f);
        control.currentlyDashing = false;
        yield return new WaitForSeconds(1.5f);
        control.canDash = true;
    }
}
