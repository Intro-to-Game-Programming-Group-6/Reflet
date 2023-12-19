using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    void Start()
    {
        LevelManager.GetInstance().Reset();
        EnemyManager.GetInstance().StartSpawning();
    }
}
