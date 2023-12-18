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
    [SerializeField] int m_selectedId;

    [SerializeField] private GameObject[] m_buttonObjects;
    private Button[] m_buttonButtons;
    private Image[] m_buttonImages;
    private TextMeshProUGUI[] m_buttonTMPs;
/*
    [SerializeField] private GameObject m_okButtonObject;
    private Button m_okButtonButton;
    private TextMeshProUGUI m_okButtonTMP;
 */ 
    private InputAction m_upArrowAction;
    private InputAction m_downArrowAction;
    private InputAction m_enterAction;

    void Awake()
    {
        if (m_buttonButtons.Length == 0)
        {
            Debug.LogError("No buttons in the `m_buttonObjects` array");
        }

        SetupSelectButtons();
        m_selectedId = -1;

        // SetupOKButton();
        // m_okButtonTMP.text = "Confirm";
    }

    void SetupButtonAt(int buttonIdx, Sprite buttonSprite, string buttonTitle = "", System.Action callback = null)
    {
        if (buttonIdx >= m_buttonButtons.Length)
        {
            Debug.LogWarning("Button id at [" + buttonIdx + "] is out of range. Buttons length: " + m_buttonButtons.Length + ".");
            return;
        }

        // TODO: implement callback
        // m_buttonButtons[buttonIdx].onClick.AddListener(() => ... callback);
        m_buttonImages[buttonIdx].sprite = buttonSprite;
        m_buttonTMPs[buttonIdx].text = buttonTitle;
    }

    void SetupSelectButtons()
    {
        int i;

        // Get Components
        for (i = 0; i < m_buttonObjects.Length; i++)
        {
            m_buttonButtons[i] = m_buttonObjects[i].GetComponent<Button>();
            m_buttonImages[i] = m_buttonObjects[i].GetComponent<Image>();
            m_buttonTMPs[i] = m_buttonObjects[i].GetComponent<TextMeshProUGUI>();
        }

        // Setup Interactabilty
        m_upArrowAction = new InputAction(binding: "<Keyboard>/rightArrow");
        m_downArrowAction = new InputAction(binding: "<Keyboard>/leftArrow");

        m_upArrowAction.performed += _ => SelectLevel(Mathf.Min(m_selectedId + 1, m_upperLimit));
        m_downArrowAction.performed += _ => SelectLevel(Mathf.Max(m_selectedId - 1, m_lowerLimit));

        // Setup Functionality
        for (i = 0; i < m_buttonButtons.Length; i++)
        {
            int x = i;
            if (i < m_size)
            {
                m_buttonButtons[i].onClick.AddListener(() => SelectLevel(x));
            }
            else
            {
                m_buttonButtons[i].interactable = false;
            }
        }
        m_upperLimit = (m_size < i) ? m_size - 1 : i - 1;
        m_lowerLimit = 0;
    }
/*
    void SetupOKButton()
    {
        if (m_okButtonObject == null) return;

        m_okButtonButton = m_okButtonObject.GetComponent<Button>();
        m_okButtonTMP = m_okButtonObject.GetComponent<TextMeshProUGUI>();

        m_enterAction = new InputAction(binding: "<Keyboard>/enter");
        m_enterAction.performed += _ => { if (m_okButtonButton.interactable) LevelManager.GetInstance().LoadLevel(m_selectedId); };

        m_okButtonButton.onClick.AddListener(() => LevelManager.GetInstance().LoadLevel(m_selectedId));
        m_okButtonButton.interactable = false;
    }
*/

    void OnEnable()
    {
        m_upArrowAction.Enable();
        m_downArrowAction.Enable();
/*
        if (m_okButtonObject == null)
            m_enterAction.Enable();
*/
    }

    void OnDisable()
    {
        m_upArrowAction.Disable();
        m_downArrowAction.Disable();
/*
        if (m_okButtonObject == null)
            m_enterAction.Disable();
*/
     }
        
    public void SelectLevel(int level) {
        Debug.Log("Selecting level " + level);
        if (m_selectedId >= 0)  m_buttonButtons[m_selectedId].OnPointerExit(null);            
        m_selectedId = level;
         m_buttonButtons[m_selectedId].Select();
        LevelManager.GetInstance().SetLevel(m_selectedId);

        // TODO: Change Scene since its been selected by... OR implement at the callback directly.
/*
        m_okButtonTMP.text = "" + (m_selectedId + 1);
        m_okButtonButton.interactable = true;
*/
    }
}
