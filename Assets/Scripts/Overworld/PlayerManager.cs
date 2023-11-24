using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public int currentHealth;
    public int maxHealth = 3;

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
}
