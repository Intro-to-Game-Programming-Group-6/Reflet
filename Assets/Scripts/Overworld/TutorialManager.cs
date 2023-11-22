using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

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

    public void UpdateState(InputAction.CallbackContext context) {
        if (context.performed && !MissionLog.GetInstance().isUpdating){
            string action = context.action.name;

            switch (currentState)
            {
                case 0:
                    if(action == "Move")
                    {
                        currentState = 1;
                        StartCoroutine(MissionLog.GetInstance().UpdateLog("Left Click to Reflect"));
                    }
                    break;
                case 1:
                    if(action == "Attack")
                    {
                        currentState = 2;
                        StartCoroutine(MissionLog.GetInstance().UpdateLog("Reflect bullets back at enemies"));
                    }
                    break;
                
            }
        }
    }
}

//0: WASD to move
//1: Click On Screen to Reflect
//2: Kill enemy
//3: Kill enemies
//4: Heal
//5: Exit
