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
        if(control.GetRigidBody().velocity.magnitude < 0.1f || control.dashCounter <= 0) yield break;

        control.BlinkParticle.Play();
        
        control.canDash = false;
        control.currentlyDashing = true;
        yield return new WaitForSeconds(control.dashCastTime);

        dashDirection = control.GetComponent<Rigidbody2D>().velocity.normalized;
        float max_range = control.blinkRange;

        control.gameObject.transform.position = ((Vector2)control.gameObject.transform.position + dashDirection * max_range);
        yield return new WaitForSeconds(control.dashDuration);
        control.currentlyDashing = false;
        control.BlinkParticle.Stop();
        if (control.dashCounter > 0)
        {
            control.dashCounter--;
            control.canDash = true;
            yield break;
        }
    }
}
