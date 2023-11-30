using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;
    [SerializeField] Transform EnemySpawn;

    private int currentState;

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
            StartCoroutine(MissionLog.GetInstance().UpdateLog("Left Click to Reflect"));
        }
    }

    public void PlayerReflected(InputAction.CallbackContext context)
    {
        if(currentState != 1)
        {
            return;
        }

        if (context.performed && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 2;
            StartCoroutine(MissionLog.GetInstance().UpdateLog("Reflect bullets back at enemies"));
            EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(3, -10, 0));
        }
    }
    public void OnPlayerKilledEnemy() {
        if (currentState != 0)
            return;
    }
    public void OnPlayerKilledEnemies() {
        if (currentState != 3) return;
        currentState = 4;
        StartCoroutine(MissionLog.GetInstance().UpdateLog("Click R to heal"));
    }


}

//0: WASD to move
//1: Click On Screen to Reflect
//2: Kill enemy
//3: Kill enemies
//4: Heal
//5: Exit
