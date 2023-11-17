using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private static Sword instance;

    public Transform pivotPoint;
    public float rotationSpeed;

    public float rotationDuration = 1.0f;
    private float rotationTimer = 0.0f;

    private bool isRotating;

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

    public static Sword GetInstance()
    {
        return instance;
    }

    void OnEnable()
    {
        isRotating = true;
    }

    void Update()
    {
        if(isRotating)
        {
            Rotate();

            rotationTimer += Time.deltaTime;

            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Rotate()
    {
        transform.RotateAround(pivotPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
