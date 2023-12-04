using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vial : MonoBehaviour
{
    private static Vial instance;

    private Slider slider;

    public void Awake() {
        if (instance == null)
        {
            instance = this;
            slider = GetComponent<Slider>();
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    void Start()
    {
        UpdateVial(0);
    }

    public static Vial GetInstance()
    {
        return instance;
    }

    public void UpdateVial(int value)
    {
        slider.value = value;
    }
}
