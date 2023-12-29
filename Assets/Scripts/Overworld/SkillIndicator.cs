using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillIndicator : MonoBehaviour
{
    // Game Objects Required
    [SerializeField] TextMeshProUGUI m_countdownTMP;

    [SerializeField] TextMeshProUGUI m_unpressedKeyTMP;
    [SerializeField] TextMeshProUGUI m_pressedKeyTMP;

    public string Key
    {
        set { m_unpressedKeyTMP.text = value; m_pressedKeyTMP.text = value; }
    }

    [SerializeField] Image m_pressedImage;
    [SerializeField] Image m_pressedImageBG;
    [SerializeField] Image m_unpressedImage;
    [SerializeField] Image m_unpressedImageBG;

    public float ImageFillAmount
    {
        set { m_pressedImage.fillAmount = value; m_unpressedImage.fillAmount = value; }
    }

    /// <example>
    /// SkillIndicator si = GetComponent<SkillIndicator>();
    /// 
    /// si.MaximumCountdown = 100f;
    /// si.ValueCountdown = 10f;
    /// si.IsPressed = false;
    /// 
    /// </example> 

    [SerializeField] 
    float m_maximumValue;
    public float MaximumCountdown
    {
        get { return m_maximumValue; }
        set { m_maximumValue = value; UpdateOnValueChanged(); }
    }
    [SerializeField]
    float m_prevValue;
    float m_Value;
    public float ValueCountdown
    {
        get { return m_Value; }
        set { m_prevValue = m_Value;  m_Value = value; UpdateOnValueChanged(); }
    }
    [SerializeField]
    bool m_isPressed;
    public bool IsPressed
    {
        get { return m_isPressed; }
        set { if (value != m_isPressed) { m_isPressed = value; UpdateColor(); } }
    }

    [SerializeField]
    private int m_skillAvailability;
    public int SkillAvailability
    {
        get { return m_skillAvailability; }
        set { m_skillAvailability = value; UpdateOnValueChanged(); }
    }

    Color m_imageColor;
    public Color ImageColor
    {
        get { return m_imageColor; }
        set { m_imageColor = value; UpdateColor(); }
    }

    private void UpdateColor()
    {
        if (m_isPressed)
        {
            m_unpressedKeyTMP.color = m_unpressedImage.color = m_unpressedImageBG.color = Color.clear; 
            m_pressedKeyTMP.color = m_pressedImage.color = m_pressedImageBG.color = m_imageColor; 
        }
        else
        {
            m_unpressedKeyTMP.color = m_unpressedImage.color = m_unpressedImageBG.color = m_imageColor;
            m_pressedKeyTMP.color = m_pressedImage.color = m_pressedImageBG.color = Color.clear;
        };
    }

    private void UpdateOnValueChanged()
    {
        if (m_prevValue >= m_maximumValue)
        {
            ImageColor = new Color(1, 1, 1, 1.0f);
            ImageFillAmount = 1;
            if (m_skillAvailability >= 1)
                m_countdownTMP.text = "" + m_skillAvailability;
            else
                m_countdownTMP.text = "";
        }
        else
        {
            ImageColor = new Color(1, 1, 1, 0.75f);
            ImageFillAmount = m_Value / m_maximumValue;
            if (m_skillAvailability >= 1)
                m_countdownTMP.text = "" + m_skillAvailability;
            else
                m_countdownTMP.text = $"{(m_maximumValue - m_Value):0.00}";


        }
    }

    void Awake()
    {
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            switch (g.name)
            {
                case "Checkmark":
                    m_unpressedImage = g.GetComponent<Image>();       
                    break;
                case "Background":
                    m_pressedImage = g.GetComponent<Image>();
                    break;
                case "Checkmark_BG":
                    m_unpressedImageBG = g.GetComponent<Image>();
                    break;
                case "Background_BG":
                    m_pressedImageBG = g.GetComponent<Image>();
                    break;

                case "KeyBackground":
                    m_pressedKeyTMP = g.GetComponent<TextMeshProUGUI>();
                    break;
                case "KeyCheckmark":
                    m_unpressedKeyTMP = g.GetComponent<TextMeshProUGUI>();
                    break;
                case "CountdownLabel":
                    m_countdownTMP = g.GetComponent<TextMeshProUGUI>();
                    break;
                default:
                    break;
            }
        }
        m_countdownTMP.color = Color.white;
    }
}
