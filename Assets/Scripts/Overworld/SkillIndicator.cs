using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillIndicator : MonoBehaviour
{
    // Game Objects Required
    TextMeshProUGUI m_countdownTMP;

    TextMeshProUGUI m_unpressedKeyTMP;
    TextMeshProUGUI m_pressedKeyTMP;

    public string Key
    {
        set { m_unpressedKeyTMP.text = value; m_pressedKeyTMP.text = value; }
    }

    Image m_pressedImage;
    Image m_pressedImageBG;
    Image m_unpressedImage;
    Image m_unpressedImageBG;

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
    float m_maximumCountdown;
    public float MaximumCountdown
    {
        get { return m_maximumCountdown; }
        set { m_maximumCountdown = value; UpdateOnValueChanged(); }
    }
    [SerializeField] 
    float m_valueCountdown;
    public float ValueCountdown
    {
        get { return m_valueCountdown; }
        set { m_valueCountdown = value; UpdateOnValueChanged(); }
    }
    [SerializeField]
    bool m_isPressed;
    public bool IsPressed
    {
        get { return m_isPressed; }
        set { if (value != m_isPressed) { m_isPressed = value; UpdateColor(); } }
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
        if (m_valueCountdown >= m_maximumCountdown)
        {
            m_valueCountdown = m_maximumCountdown;
            ImageColor = new Color(1, 1, 1, 1.0f);
            ImageFillAmount = 1;
            m_countdownTMP.text = "";
        }
        else
        {
            ImageColor = new Color(1, 1, 1, 0.5f);
            ImageFillAmount = (m_maximumCountdown - m_valueCountdown) / m_maximumCountdown;
            m_countdownTMP.text = $"{m_valueCountdown:0.00}";
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
                case "Label":
                    m_countdownTMP = g.GetComponent<TextMeshProUGUI>();
                    break;
                default:
                    break;
            }
        }
    }
}
