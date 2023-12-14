using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AoEHealing AoeHeal;
    public GameObject AoEPrefab;
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
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
