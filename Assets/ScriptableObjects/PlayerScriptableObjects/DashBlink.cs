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
        Debug.Log("Dash blinking activated");
        if(control.GetRigidBody().velocity.magnitude < 0.1f) yield break;

        control.BlinkParticle.Play();
        
        control.canDash = false;
        control.currentlyDashing = true;
        yield return new WaitForSeconds(control.dashDuration);

        dashDirection = control.GetComponent<Rigidbody2D>().velocity.normalized;
        float max_range = control.blinkRange;

        control.GetRigidBody().position = (control.GetRigidBody().position + dashDirection * max_range);
        yield return new WaitForSeconds(control.dashDuration);
        control.currentlyDashing = false;
        control.BlinkParticle.Stop();
        yield return new WaitForSeconds(control.dashCooldown);
        control.canDash = true;
    }
}
