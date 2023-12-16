using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCircleLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public float totalHealing;
    public float healing_duration;
    public float current_time;
    private bool playerInsideCircle = false;
    private float healthIncrement;
    private float heal_done;
    public void SetProperties(float totalHeal, float healTime, float radius)
    {
        totalHealing = totalHeal;
        healing_duration = healTime;
        transform.localScale *= radius;
        Debug.Log("Healing total is: " + totalHealing);
        Debug.Log("Healing duration is: " + healing_duration);
        heal_done = 0f;
        
    }

    private void OnEnable()
    {
        current_time = 0f;
        PlayerManager.GetInstance().EmptyVial();
    }

    void Update()
    {
        current_time += Time.deltaTime;
        if (current_time >= healing_duration)
        {
            Debug.Log("Healing done: " + heal_done);
            Destroy(gameObject);
            return;
        }
        if (playerInsideCircle)
        {
            healthIncrement = totalHealing / healing_duration * Time.deltaTime;
            //Debug.Log("Increase health by: " + healthIncrement);
            PlayerManager.GetInstance().AdjustHealth(healthIncrement);
            heal_done += healthIncrement;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCircle = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCircle = false;
        }
    }
}
