using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Dash NormalDash;
    public DashBlink BlinkDash;
    public void StartDash(PlayerControlScript main_controller)
    {
        main_controller.trail.emitting = true;
        switch (main_controller.DashID)
        {
            case 1:
                StartCoroutine(NormalDash.GoDash(main_controller));
                break;
            case 2:
                StartCoroutine(BlinkDash.GoDash(main_controller));
                break;
            case 3:
                break;
        }
    }

    void UpgradeDash(int upgrade_id, PlayerControlScript main_controller, float bonus)
    {
        switch (upgrade_id)
        {
            case 1:
                main_controller.DashCooldown += bonus;
                break;
            case 2:
                main_controller.DashDuration += bonus;
                break;
            case 3:
                main_controller.DashSpeed += bonus;
                break;
        }
    }
}
