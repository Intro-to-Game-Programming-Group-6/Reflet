using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    private int currentState;
    private int tutorialEnemyCount = 3;

    private Coroutine spawnCoroutine = null;
    private GameObject tutorialEnemy = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static TutorialManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        currentState = 0;
        StartCoroutine(MissionLog.GetInstance().UpdateLog("Press WASD to move"));
    }

    public void UpdateLog(string message)
    {
        StartCoroutine(MissionLog.GetInstance().UpdateLog(message));
    }

    public void PlayerMoved(InputAction.CallbackContext context)
    {
        if(currentState != 0)
        {
            return;
        }

        if (context.performed && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 1;
            StartCoroutine(MissionLog.GetInstance().UpdateLog("Press Space to Dash"));
        }
    }

    public void PlayerDashed(InputAction.CallbackContext context)
    {
        if(currentState != 1)
        {
            return;
        }

        if (context.performed && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 2;
            StartCoroutine(MissionLog.GetInstance().UpdateLog("Left Click to Reflect"));
        }
    }

    public void PlayerReflected(InputAction.CallbackContext context)
    {
        if(currentState != 2)
        {
            return;
        }

        if (context.performed && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 3;
            StartCoroutine(MissionLog.GetInstance().UpdateLog("Reflect bullets back at enemies"));
            if(spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnEnemies());
            }
        }
    }

    public void PlayerKilledEnemies()
    {
        if(currentState != 3)
        {
            return;
        }

        currentState = 4;
        PlayerManager.GetInstance().AddVialPoint(10);
        StartCoroutine(MissionLog.GetInstance().UpdateLog("Click R to heal"));
    }

    public void PlayerHeal(InputAction.CallbackContext context)
    {
        if(currentState != 4)
        {
            return;
        }

        if (context.performed && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 5;
            StartCoroutine(MissionLog.GetInstance().UpdateLog("Enter portal to exit tutorial"));

        }
    }

    IEnumerator SpawnEnemies()
    {
        Vector3 enemySpawnPoint1 = new Vector3(3, -10, 0);
        Vector3 enemySpawnPoint2 = new Vector3(-8, -10, 0);

        bool spawnPoint = true;
        int currentEnemyCount = 0;

        while(currentEnemyCount != tutorialEnemyCount || EnemyManager.GetInstance().enemyCount != 0)
        {
            if(EnemyManager.GetInstance().enemyCount == 0)
            {
                currentEnemyCount += 1;

                yield return new WaitForSeconds(1.5f);

                if(spawnPoint)
                {
                    tutorialEnemy = EnemyManager.GetInstance().SpawnEnemy(0, enemySpawnPoint1); 
                    spawnPoint = false;
                }
                else
                {
                    tutorialEnemy = EnemyManager.GetInstance().SpawnEnemy(0, enemySpawnPoint2);
                    spawnPoint = true;
                }
                
            }
            yield return null;
        }

        if(EnemyManager.GetInstance().enemyCount == 0)
        {
            PlayerKilledEnemies();
        }
        yield return null;
    }
}
