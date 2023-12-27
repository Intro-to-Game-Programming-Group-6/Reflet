using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Dash NormalDash;
    public DashBlink BlinkDash;
    public void StartDash(PlayerControlScript mainController)
    {
        mainController.trail.emitting = true;
        mainController.shields_up.pitch = 1f;
        switch (mainController.dashID)
        {
            case 1:
                if(mainController.dashCounter > 0)
                    mainController.shields_up.PlayOneShot(mainController.heal_audios[3]);
                StartCoroutine(NormalDash.GoDash(mainController));
                break;
            case 2:
                if (mainController.dashCounter > 0)
                    mainController.shields_up.PlayOneShot(mainController.heal_audios[3]);
                StartCoroutine(BlinkDash.GoDash(mainController));
                break;
            case 3:
                break;
        }
        //mainController.shields_up.pitch = 1.4f;
    }

    void UpgradeDash(int upgrade_id, PlayerControlScript mainController, float bonus)
    {
        switch (upgrade_id)
        {
            case 1:
                mainController.dashCooldown += bonus;
                break;
            case 2:
                mainController.dashDuration += bonus;
                break;
            case 3:
                mainController.dashSpeed += bonus;
                break;
        }
    }
}
