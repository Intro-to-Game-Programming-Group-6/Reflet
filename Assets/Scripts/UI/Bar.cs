using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class Bar: MonoBehaviour
{
    [Header("Bar Data")]
    [SerializeField] public int maxBar;
    [SerializeField] public int currentBar;

    [Header("Bar UI")]
    [SerializeField] private GameObject _aBarOject;

    [Header("Bar Sprite")]
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;

    [Header("Bar Pos")]
    [SerializeField] private float offsetBetween;

    List<GameObject> bars;

    void Awake()
    {
        print("yes");
        bars = new List<GameObject>();

        if (maxBar < 1)
            Debug.LogError("Max Bar must be greater than 0");
        else if (currentBar < 0 || currentBar > maxBar)

        for (int i = 0; i < maxBar; i++)
        {
            Debug.Log(gameObject.GetComponent<RectTransform>().position.x);
            GameObject container = Instantiate(_aBarOject, gameObject.transform.position + new Vector3(i * offsetBetween + gameObject.GetComponent<RectTransform>().position.x, gameObject.GetComponent<RectTransform>().position.x, 0f), Quaternion.identity, gameObject.transform);
            bars.Add(container);
        }

        AdjustBarUI();
    }


    public void UpdateBarUI(int updatedMaxBar)
    {
        maxBar = updatedMaxBar;

        if (maxBar < updatedMaxBar)
        {
            IncreaseBarUI();
        }
        else if (maxBar > updatedMaxBar)
        {
            DecreaseBarUI();
        }
    }

    public void AdjustBarUI(int updatedCurrentBar=-1)
    {
        if (updatedCurrentBar >= 0) 
            currentBar = updatedCurrentBar;

        for (int i = 0; i < maxBar; i++)
        {
            bars[i].GetComponent<Image>().sprite = (i < currentBar) ? fullSprite : emptySprite;
        }
    }

    private void IncreaseBarUI()
    {
        for (int i = bars.Count; i < maxBar; i++)
        {
            GameObject container = Instantiate(_aBarOject, gameObject.transform.position + new Vector3(i * offsetBetween + gameObject.GetComponent<RectTransform>().position.x, gameObject.GetComponent<RectTransform>().position.x, 0f), Quaternion.identity, gameObject.transform);
            bars.Add(container);
        }
    }

    private void DecreaseBarUI()
    {
        for (int i = maxBar - 1; i >= bars.Count; i--)
        {
            Destroy(bars[i]);
        }
    }
}
