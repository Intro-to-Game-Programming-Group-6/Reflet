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
public class BarController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Size Delta")]
    [SerializeField] private float m_height;

    [Header("Bar Pixel Size")]
    [SerializeField] private float m_minBarSize;
    [SerializeField] private float m_perBarSize;

    [SerializeField] private int m_maxBar;
    [SerializeField] private int m_curBar;

    private Slider m_slider;
    private RectTransform m_rt;

    /*
    <example>
        public class Example : MonoBehavior
        {
            [SerializeField] private BarController m_barController;
            void Start()
            {
                m_barController.SetMax(10);
                m_barController.SetValue(5);
            }
        }
    </example>
    */
    void Awake()
    {
        m_slider = GetComponent<Slider>();
        m_rt = GetComponent<RectTransform>();
        
        m_slider.minValue = 0;
        m_slider.wholeNumbers = true;

   
    }

    public void SetMax(int value)
    {
        m_maxBar = value;
        float _width = m_minBarSize + m_perBarSize * (m_maxBar - 1);
        m_rt.sizeDelta = new Vector2(_width, m_height);
        m_slider.maxValue = m_maxBar;
    }
    
    public void AddValue(int value)
    {
        m_curBar += value;
        if (m_curBar > m_maxBar)
        {
            m_curBar = m_maxBar;
        }else if (m_curBar < 0)
        {
            m_curBar = 0;
        }
        m_slider.value = m_curBar;
    }

    public void SubValue(int value)
    {
        AddValue(-value);
    }
    
    public void SetValue(int value)
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
