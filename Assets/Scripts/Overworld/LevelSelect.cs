using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class LevelSelect : MonoBehaviour
{
    int m_upperLimit;
    int m_lowerLimit;
    [SerializeField] int m_size;
    [SerializeField] int m_levelId;
    [SerializeField] private Button[] m_buttons;
    [SerializeField] private Button m_startButton;
    [SerializeField] private TextMeshProUGUI m_levelIdText;
    
    private InputAction m_upArrowAction;
    private InputAction m_downArrowAction;
    private InputAction m_enterAction;

    void Awake()
    {
        m_upArrowAction = new InputAction(binding: "<Keyboard>/upArrow");
        m_downArrowAction = new InputAction(binding: "<Keyboard>/downArrow");
        m_enterAction = new InputAction(binding: "<Keyboard>/enter");

        m_upArrowAction.performed += _ => SelectLevel(Mathf.Min(m_levelId + 1, m_upperLimit));
        m_downArrowAction.performed += _ => SelectLevel(Mathf.Max(m_levelId - 1, m_lowerLimit));
        m_enterAction.performed += _ => { if (m_startButton.interactable) LevelManager.GetInstance().LoadLevel(m_levelId); };

        m_startButton.onClick.AddListener(() => LevelManager.GetInstance().LoadLevel(m_levelId));
        m_startButton.interactable = false;
        
        int i = 0;
        foreach (Button btns in m_buttons)
        {
            int x = i;
            if (i < m_size) {
                btns.onClick.AddListener(() => SelectLevel(x));
                i++;
            }
            else {
                btns.interactable = false;
            }
        }

        m_upperLimit = (m_size < i) ? m_size - 1 : i - 1;
        m_lowerLimit = 0; 

        m_levelId = -1;
        m_levelIdText.text = "-";

        if (m_buttons.Length == 0)
        {
            Debug.LogError("No buttons in the m_buttons array");
        }
    }

    void OnEnable()
    {
        m_upArrowAction.Enable();
        m_downArrowAction.Enable();
        m_enterAction.Enable();
    }

    void OnDisable()
    {
        m_upArrowAction.Disable();
        m_downArrowAction.Disable();
        m_enterAction.Disable();
    }
        
    public void SelectLevel(int level) {
        Debug.Log("Selecting level " + level);
        if (m_levelId >= 0) m_buttons[m_levelId].OnPointerExit(null);            
        m_levelId = level;
        m_buttons[m_levelId].Select();
        LevelManager.GetInstance().SetLevel(m_levelId);
        m_levelIdText.text = "" + (m_levelId + 1);
        m_startButton.interactable = true;
    }
}
