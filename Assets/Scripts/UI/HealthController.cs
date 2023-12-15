using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used to control the bar UI with a pixel of 10 pixel per unit.
/// </summary>

[RequireComponent(typeof(Slider), typeof(RectTransform))]
[ExecuteAlways]
public class HealthController : MonoBehaviour
{
    [Header("Size Delta")]
    [SerializeField] private float m_height;

    [Header("Bar Pixel Size")]
    [SerializeField] private float m_minBarSize;
    [SerializeField] private float m_perBarSize;
    [SerializeField] private float m_maxBar;
    [SerializeField] private float m_curBar;

    private Slider m_slider;
    private RectTransform m_rt;

    void Awake()
    {
        m_slider = GetComponent<Slider>();
        m_rt = GetComponent<RectTransform>();
        
        m_slider.minValue = 0;
        m_slider.wholeNumbers = true;
    }

    public void SetMax(float value)
    {
        m_maxBar = value;
        // float _width = m_minBarSize + m_perBarSize * (m_maxBar - 1);
        // m_rt.sizeDelta = new Vector2(_width, m_height);
        m_slider.maxValue = m_maxBar;
    }
    
    public void AddValue(float value)
    {
        m_curBar += value;
        if (m_curBar > m_maxBar)
        {
            m_curBar = m_maxBar;
        }
        else if (m_curBar < 0)
        {
            m_curBar = 0;
        }
        m_slider.value = m_curBar;
    }

    public void SubValue(float value)
    {
        AddValue(-value);
    }
    
    public void SetValue(float value)
    {
        m_curBar = value;
        if (m_curBar > m_maxBar)
        {
            m_curBar = m_maxBar;
        }
        else if (m_curBar < 0)
        {
            m_curBar = 0;
        }
        m_slider.value = m_curBar;
    }
}
