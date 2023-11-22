using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private static HealthManager instance;
    public int currentHealth, maxHealth = 3;
    [SerializeField] List<Image> hearts;
    [SerializeField] List<Sprite> sprites;
    private void Awake() {
        currentHealth = maxHealth;
        if (instance == null) {
            instance = this;
            return;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public static HealthManager GetInstance() {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maxHealth; i++) {
            hearts[i].sprite = sprites[(i < currentHealth) ? 0 : 1];
        }
    }



    public void AddHealth(int deltaHealth) {
        currentHealth += deltaHealth;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        if (currentHealth < 0) {
            currentHealth = 0;
            //Game Over Here
        }
    }
}
