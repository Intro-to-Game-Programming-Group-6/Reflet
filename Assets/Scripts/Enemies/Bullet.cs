using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = PlayerControlScript.GetInstance().gameObject.transform;

        AimAtPlayer();
    }

    void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }

    void AimAtPlayer()
    {
        if (playerTransform != null)
        {
            Vector2 directionToPlayer = playerTransform.position - transform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg);
        }
    }
}
