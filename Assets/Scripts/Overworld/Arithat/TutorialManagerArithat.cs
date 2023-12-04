using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TutorialManagerArithat : MonoBehaviour
{
    private static TutorialManagerArithat instance;

    private int currentState;

    public GameObject tutorialEnemy;

    void Awake()
    {
        if(instance == null)
            instance = this;
        if(instance != this)
            Destroy(gameObject);
    }

    public static TutorialManagerArithat GetInstance() {
        return instance;
    }

    void Start() {
        currentState = 0;
    }

    public UnityEvent PlayerLearnedMove;
    public UnityEvent PlayerLearnedReflect;
    public UnityEvent PlayerLearnedAttack;
    public UnityEvent PlayerVialFull;
    public UnityEvent PlayerHealed;

    public void PlayerMoves() {
        if(currentState == 0 && !MissionLog.GetInstance().isUpdating) {
            currentState = 1;
            PlayerLearnedMove.Invoke();
            UpdateLog("Left click to reflect");
        }
    }

    public void PlayerReflects() {
        if (currentState == 1 && !MissionLog.GetInstance().isUpdating) {
            currentState = 2;
            PlayerLearnedReflect.Invoke();
            UpdateLog("Reflect bullets back at enemies");
        }
    }

    public void PlayerAttacks() {
        if(currentState == 2 && EnemyManager.GetInstance().enemyCount == 0 && !MissionLog.GetInstance().isUpdating) {
            currentState = 3;
            PlayerLearnedAttack.Invoke();
            UpdateLog("Kill more enemies to fill up the vial");
        }
    }

    public void PlayerFillsVial() {
        if(currentState == 3 && PlayerManager.GetInstance().VialFullState() && !MissionLog.GetInstance().isUpdating) {
            currentState = 4;
            PlayerVialFull.Invoke();
            UpdateLog("Click R to heal");
        }
    }

    public void PlayerHeals() {
        if(currentState == 4 && !MissionLog.GetInstance().isUpdating) {
            currentState = 5;
            PlayerHealed.Invoke();
            UpdateLog("End of the tutorial");
        }
    }

    public void UpdateLog(string message) {
        StartCoroutine(MissionLog.GetInstance().UpdateLog(message));
    }

    public void SpawnFirstEnemy() {
        EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(3, -10, 0));
    }

    public void SpawnNewEnemy() {
        if(!PlayerManager.GetInstance().VialFullState()) {
            EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(3, -10, 0));
        } else {
            PlayerFillsVial();
        }
    }
}

//0: WASD to move
//1: Click On Screen to Reflect
//2: Kill enemy
//3: Kill enemies
//4: Heal
//5: Exit
