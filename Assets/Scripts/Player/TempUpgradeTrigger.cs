using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUpgradeTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public string upgrade_mode;
    PlayerControlScript player_control_info;
    PlayerManager player_manager_info;
    ReflectUpgradeType randomUpgrade;
    HealUpgradeType randomHealUpgrade;
    void Start()
    {
        player_manager_info = PlayerManager.GetInstance();
        player_control_info = PlayerControlScript.GetInstance();
        randomUpgrade = (ReflectUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(ReflectUpgradeType)).Length);
        randomHealUpgrade = (HealUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(HealUpgradeType)).Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum HealUpgradeType
    {
        HealID,
        VialCapacity,
        VialResourceDrain,
    }

    void GenerateHealUpgrades()
    {
        HealUpgradeType randomUpgrade = (HealUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(HealUpgradeType)).Length);

        switch (randomUpgrade)
        {
            case HealUpgradeType.HealID:
                HandleSpecificHealAction();
                break;

            case HealUpgradeType.VialCapacity:
                float IncreaseCapacity = Random.Range(10f, 30f);
                player_manager_info.maxVial *= (1 + IncreaseCapacity / 100);
                break;

            case HealUpgradeType.VialResourceDrain:
                float ReduceUsage = Random.Range(10f, 30f);
                player_manager_info.useVial *= (1 - ReduceUsage / 100);
                break;
            default:
                break;

        }
    }

    void HandleSpecificHealAction()
    {
        int ChooseHealingID = Random.Range(2, 5);
        if (ChooseHealingID == player_control_info.HealID)
        {
            if (ChooseHealingID == 2) //AoE healing HealActionManager id 2
            {
                float IncreaseRadius = Random.Range(10f, 30f);
                float IncreaseAoeHeal = Random.Range(10f, 30f);
                float CutAoeHealTime = Random.Range(10f, 30f);
                player_control_info.aoeHealRadius *= (1f + IncreaseRadius / 100);
                player_control_info.aoeHealTime *= (1f - CutAoeHealTime / 100);
                player_control_info.aoeHealTotal *= (1f + IncreaseAoeHeal / 100);
            }
            else if (ChooseHealingID == 3)//HealActionManager Disintegration
            {
                float IncreaseDisintegrationDuration = Random.Range(10f, 30f);
                player_control_info.DisintegrationDuration *= (1f + IncreaseDisintegrationDuration / 100);
            }else if(ChooseHealingID == 4)//HealActionManager ReflectShield
            {
                player_control_info.ReflectShieldHP += 1;
            }
                    
        }
        else
            player_control_info.HealID = ChooseHealingID;
    }
    public enum ReflectUpgradeType
    {
        RotatingShield,
        Duplication,
        StaminaRegen,
        StaminaCapacity,
        StaminaDecay
    }
    void GenerateReflectUpgrades()
    {
        switch (randomUpgrade)
        {
            case ReflectUpgradeType.StaminaCapacity:
                float IncreaseCapacity = Random.Range(10f, 30f);
                float newval = player_manager_info.maxStamina * (1 + IncreaseCapacity / 100);
                player_manager_info.ModifyStaminaCapacity(Mathf.Round(newval));
                break;
            case ReflectUpgradeType.StaminaRegen:
                float IncreaseRegenRate = Random.Range(10f, 30f);
                player_control_info.stamina_regen *= (1 + IncreaseRegenRate / 100);
                break;
            case ReflectUpgradeType.StaminaDecay:
                float ReduceDecayRate = Random.Range(10f, 30f);
                player_control_info.stamina_decay *= (1 - ReduceDecayRate / 100);
                break;
            case ReflectUpgradeType.Duplication:
                if (player_manager_info.multiply)
                    player_manager_info.bulletMultiplier += 1;
                else
                    player_manager_info.multiply = true;
                break;
            case ReflectUpgradeType.RotatingShield:
                if (!player_control_info.mirrorRotate)
                {
                    player_control_info.mirrorRotate = true;
                    player_control_info.numShields += 1;
                    player_control_info.CreateOrbitingShields();
                }
                else
                {
                    player_control_info.numShields += 1;
                    player_control_info.CreateOrbitingShields();
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(upgrade_mode == "reflect")
            {
                Debug.Log("Player accessing reflect upgrade");
                Debug.Log("Player is currently locked to using: " + randomUpgrade);
                GenerateReflectUpgrades();
                Debug.Log("Upgrade Applied");
            }
            else if(upgrade_mode == "heal")
            {
                Debug.Log("Player accessing heal upgrade");
                Debug.Log("Player is currently locked to using: " + randomHealUpgrade);
                GenerateHealUpgrades();
                Debug.Log("Upgrade Applied");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (upgrade_mode == "reflect")
            {
                Debug.Log("Randomizing Reflect Upgrade");
                randomUpgrade = (ReflectUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(ReflectUpgradeType)).Length);
                Debug.Log("Player is currently locked to using: " + randomUpgrade);
            }
            else if (upgrade_mode == "heal")
            {
                Debug.Log("Randomizing Heal Upgrade");
                randomHealUpgrade = (HealUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(HealUpgradeType)).Length);
                Debug.Log("Player is currently locked to using: " + randomHealUpgrade);
            }
        }
    }
}
