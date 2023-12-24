using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    //Used for testing purpose.
    public void SpawnEnemy() {
        EnemyManager.GetInstance().SpawnEnemy(0, new Vector3(0, 0, 0));
    }
}
