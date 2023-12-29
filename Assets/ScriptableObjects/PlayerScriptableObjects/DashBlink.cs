using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[CreateAssetMenu]
public class DashBlink : BaseAbillity
{
    Vector2 dashDirection;
    public IEnumerator GoDash(PlayerControlScript control, DashManager manager)
    {
        if (manager.dashAvailability <= 0) yield break;
        Debug.Log("Dash blinking activated");
        manager.blinkParticle.Play();

        manager.canDash = false;
        manager.isDashing = true;
        yield return new WaitForSeconds(manager.dashCastTime);

        Vector2 dashDirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - control.GetRigidBody().transform.position;
        float mouseRange = dashDirection.magnitude;
        float allowedRange = Mathf.Min(manager.blinkRange, mouseRange);

        dashDirection.Normalize(); // Normalize the direction

        // Teleportation
        control.gameObject.transform.position = (Vector2)control.gameObject.transform.position + dashDirection * allowedRange;
        yield return new WaitForSeconds(manager.dashDuration);
        manager.isDashing = false;
        manager.blinkParticle.Stop();

        if (manager.dashAvailability > 0)
        {
            manager.dashAvailability--;
            manager.canDash = true;
            yield break;
        }
    }
}
