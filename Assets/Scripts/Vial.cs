using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vial : MonoBehaviour
{
    public static Vial instance;
    public Slider slider;
    int vialPoint = 0;
    public int vialMaxPoint = 10;
    public void Awake() {
        if (instance == null) {
            instance = this;
        }
        if (instance != this) {
            Destroy(gameObject);
        }
        slider = GetComponent<Slider>();
    }

    public void Start() {
        vialPoint = 0;
        vialMaxPoint = 10;
    }

    public static Vial GetInstance() {
        return instance;
    }

    // Update is called once per frame
    void Update() {
        slider.value = vialPoint;
    }

    public void AddVialPoint(int deltaPoint) {
        vialPoint += deltaPoint;
        if(vialPoint > vialMaxPoint) {
            vialPoint = vialMaxPoint;
        }
    }

    public void UseVial() {
        if(vialPoint < vialMaxPoint)
            return;
        PlayerManager.GetInstance().AdjustHealth(1);
        vialPoint = 0;
    }

    public bool isFull() {
        return vialPoint == vialMaxPoint;
    }
}
