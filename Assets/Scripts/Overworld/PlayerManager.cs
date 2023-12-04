using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public int currentHealth;
    public int maxHealth = 3;

    private int vialPoint = 0;
    public int vialMaxPoint = 10;

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

        if(vialPoint > vialMaxPoint)
        {
            vialPoint = vialMaxPoint;
        }

        Vial.GetInstance().UpdateVial(vialPoint);
    }

    public void UseVial()
    {
        if(vialPoint < vialMaxPoint)
        {
            return;
        }
            
        AdjustHealth(1);
        vialPoint = 0;
        Vial.GetInstance().UpdateVial(vialPoint);
    }

    public bool VialFullState()
    {
        return vialPoint == vialMaxPoint;
    }
}
