using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    private int currentState;

    [Header("Unity Events")]
    [SerializeField] UnityEvent PlayerLearnedMove;
    [SerializeField] UnityEvent PlayerLearnedReflect;
    [SerializeField] UnityEvent PlayerLearnedAttack;
    [SerializeField] UnityEvent PlayerKilledEnemy;
    [SerializeField] UnityEvent PlayerVialFull;
    [SerializeField] UnityEvent PlayerHealed;

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

    public void PlayerMoves()
    {
        if(currentState == 0 && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 1;
            PlayerLearnedMove.Invoke();
            UpdateLog("Left click to reflect");
        }
    }

    public void PlayerReflects()
    {
        if (currentState == 1 && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 2;
            PlayerLearnedReflect.Invoke();
            UpdateLog("Reflect bullets back at enemies");
        }
    }

    public void PlayerAttacks()
    {
        if(currentState == 2 && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 3;
            PlayerLearnedAttack.Invoke();
            UpdateLog("Kill more enemies to fill up the vial");
        }
    }

    public void PlayerFillsVial()
    {
        if(currentState == 3 && PlayerManager.GetInstance().VialFullState() && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 4;
            PlayerVialFull.Invoke();
            UpdateLog("Click R to heal");
        }
    }

    public void PlayerHeals()
    {
        if(currentState == 4 && !MissionLog.GetInstance().isUpdating)
        {
            currentState = 5;
            PlayerHealed.Invoke();
            UpdateLog("End of the tutorial");
        }
    }

    public void UpdateLog(string message)
    {
        StartCoroutine(MissionLog.GetInstance().UpdateLog(message));
    }

    public void SpawnFirstEnemy()
    {
        EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(3, -10, 0));
    }

    public void SpawnNewEnemy()
    {
        if(!PlayerManager.GetInstance().VialFullState())
        {
            EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(3, -10, 0));
        }
        else
        {
            PlayerFillsVial();
        }
    }

    // void Update()
    // {
    //     if(EnemyManager.GetInstance().enemyCount == 0 && currentState == 3)
    //     {
    //         OnPlayerKilledEnemies();
    //     }
    // }

    // public void PlayerMoved(InputAction.CallbackContext context)
    // {
    //     if(currentState != 0)
    //     {
    //         return;
    //     }

    //     if (context.performed && !MissionLog.GetInstance().isUpdating)
    //     {
    //         currentState = 1;
    //         StartCoroutine(MissionLog.GetInstance().UpdateLog("Press Space to Dash"));
    //     }
    // }

    // public void PlayerDashed(InputAction.CallbackContext context)
    // {
    //     if(currentState != 1)
    //     {
    //         return;
    //     }

    //     if (context.performed && !MissionLog.GetInstance().isUpdating)
    //     {
    //         currentState = 2;
    //         StartCoroutine(MissionLog.GetInstance().UpdateLog("Left Click to Reflect"));
    //     }
    // }

    // public void PlayerReflected(InputAction.CallbackContext context)
    // {
    //     if(currentState != 2)
    //     {
    //         return;
    //     }

    //     if (context.performed && !MissionLog.GetInstance().isUpdating)
    //     {
    //         currentState = 3;
    //         StartCoroutine(MissionLog.GetInstance().UpdateLog("Reflect bullets back at enemies"));
    //         EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(3, -10, 0));
    //     }
    // }

    // public void OnPlayerKilledEnemies()
    // {
    //     currentState = 4;
    //     PlayerManager.GetInstance().AddVialPoint(10);
    //     StartCoroutine(MissionLog.GetInstance().UpdateLog("Click R to heal"));
    // }
}

// 0: WASD to move
// 1: Click On Screen to Reflect
// 2: Kill enemy
// 3: Kill enemies
// 4: Heal
// 5: Exit
