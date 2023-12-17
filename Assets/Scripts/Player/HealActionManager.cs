using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AoEHealing AoeHeal;
    public DisintegrationShield Disintegration;
    public ReflectShield ShieldReflect;
    public GameObject AoEPrefab, ReflectShieldPrefab, DisintegratePrefab;
    public void StartHeal(PlayerControlScript mainController)
    {
        switch (mainController.HealID)
        {
            case 1:
                PlayerManager.GetInstance().Heal(1);
                break;
            case 2:
                AoeHeal.CreateField(AoEPrefab, mainController);
                break;
            case 3:
                Disintegration.CreateField(DisintegratePrefab, mainController);
                break;
            case 4:
                ShieldReflect.CreateField(ReflectShieldPrefab, mainController);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
