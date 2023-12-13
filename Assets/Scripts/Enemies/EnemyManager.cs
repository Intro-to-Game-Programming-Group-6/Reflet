using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    [SerializeField] private List<GameObject> EnemyPrefabs;

    public int enemyCount;
    public bool enemyWaveStarted;
    public Transform upperCorner;
    public Transform lowerCorner;
    public UnityEvent EnemySpawned;
    public UnityEvent EnemyDie;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            
    }

    public void Start()
    {
        enemyCount = 0;
        enemyWaveStarted = false;
    }

    public static EnemyManager GetInstance() 
    {
        return instance;
    }

    public GameObject SpawnEnemy(int index, Vector3 position)
    {
        GameObject enemy = EnemyPrefabs[index];
        Instantiate(enemy, position, Quaternion.identity);
        enemyCount++;
        EnemySpawned?.Invoke();
        return enemy;
    }

    /*
    public void StartEnemyWave(int enemyNum) {
        enemyWaveStarted = true;
        for (int i = 0; i < enemyNum; i++) {
            float x = Random.Range(lowerCorner.position.x, upperCorner.position.x);
            float y = Random.Range(lowerCorner.position.y, upperCorner.position.y);
            SpawnEnemy(Random.Range(0, EnemyPrefabs.Count), new Vector3(x, y, 0));
        }
    }

    IEnumerator ListenEnemyExtinct(System.Action callback) {
        yield return new WaitUntil(() => enemyCount == 0);
        Debug.Log("Enemy Extincted");
        callback.Invoke();
        yield return null;
    }
    */

    public void HandleEnemyDeath()
    {
        enemyCount--;
        EnemyDie?.Invoke();
    }
}
