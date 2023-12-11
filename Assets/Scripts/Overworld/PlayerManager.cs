using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public int currentHealth;
    public int maxHealth = 3;

    public int vialPoint;
    public int vialMaxPoint = 10;

    public bool canHeal;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static PlayerManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        currentHealth = maxHealth;
        vialPoint = 0;
        canHeal = false;
    }

    public void AdjustHearts(int deltaHeart) {
        maxHealth += deltaHeart;

        AdjustHealth(0);
    }

    public void AdjustHealth(int deltaHealth) {
        currentHealth += deltaHealth;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void AddVialPoint(int deltaPoint)
    {
        vialPoint += deltaPoint;

        if(vialPoint >= vialMaxPoint)
        {
            canHeal = true;
            vialPoint = vialMaxPoint;
        }

        Vial.GetInstance().UpdateVial(vialPoint);
    }

    public bool VialFullState()
    {
        return vialPoint == vialMaxPoint;
    }

    public void Heal()
    {
        canHeal = false;
        AdjustHealth(1);
        vialPoint = 0;
        Vial.GetInstance().UpdateVial(vialPoint);   
    }
}
