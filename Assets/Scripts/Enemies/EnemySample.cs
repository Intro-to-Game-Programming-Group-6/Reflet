using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySample : MonoBehaviour
{
    public GameObject bullet;
    public Vector3 offset;

    void Start()
    {
        StartCoroutine(ShootBullets());
    }

    void Shoot()
    {
        Instantiate(bullet, gameObject.transform.position + offset, Quaternion.Euler(0, 0, 0));
    }

    IEnumerator ShootBullets()
    {
        while(true)
        {
            yield return new WaitForSeconds(3.0f);
            Shoot();
        }
    }
}
