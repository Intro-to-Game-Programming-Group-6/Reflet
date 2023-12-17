using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SelectController : MonoBehaviour
{
    int m_upperLimit;
    int m_lowerLimit;
    [SerializeField] int m_size;
    [SerializeField] int m_selectedId;
    [SerializeField] private GameObject[] m_buttonObjects;
    private Button[] m_buttons;
    private Image[] m_images;
    private TextMeshProUGUI[] m_TMPs;

    [SerializeField] private Button m_okButton;
    [SerializeField] private TextMeshProUGUI m_selectedIdText;
    
    private InputAction m_upArrowAction;
    private InputAction m_downArrowAction;
    private InputAction m_enterAction;

    void Awake()
    {
        SetupButtonsComponent();
        Debug.Log ("Setup Comp. Done");

        if (m_buttons.Length == 0) Debug.LogError("No buttons in the m_buttons array");

        if (m_okButton != null){
            SetupOKButton();
            Debug.Log ("Setup OK Done");
        }

        SetupSelectionButton();
        Debug.Log ("Setup Button Done");

        InitializeVariable();
        Debug.Log ("Init  Done");
    }
    private void SetupButtonsComponent() {
        int len = m_buttonObjects.Length;
        m_buttons = new Button[len];
        m_images = new Image[len];
        m_TMPs = new TextMeshProUGUI[len];

        for(int i = 0; i < len; i++) {
            m_buttons[i] = m_buttonObjects[i].GetComponent<Button>();
            m_images[i] = m_buttonObjects[i].GetComponent<Image>();
            m_TMPs[i] = m_buttonObjects[i].GetComponent<TextMeshProUGUI>();

            if (m_buttons[i] == null) Debug.LogError("No button component in the m_buttons array");
            if (m_images[i] == null) Debug.LogError("No image component in the m_images array");
            if (m_TMPs[i] == null) Debug.LogError("No text component in the m_TMPs array");
        }

    }
    private void SetupSelectionButton() {
        // Keyboard Input Setup
        m_upArrowAction = new InputAction(binding: "<Keyboard>/upArrow");
        m_downArrowAction = new InputAction(binding: "<Keyboard>/downArrow");
        m_upArrowAction.performed += _ => SelectLevel(Mathf.Min(m_selectedId + 1, m_upperLimit));
        m_downArrowAction.performed += _ => SelectLevel(Mathf.Max(m_selectedId - 1, m_lowerLimit));

        // Interface & Behavior Setup
        int i;
        for (i = 0; i < m_buttons.Length; i++) {
            int x = i;
            if (i < m_size) {
                m_buttons[i].onClick.AddListener(() => SelectLevel(x));
            }
            else {
                m_buttons[i].interactable = false;
            }
        }

        // Limit Setup
        m_upperLimit = (m_size < i) ? m_size - 1 : i - 1;
        m_lowerLimit = 0;
    }
    private void SetupOKButton() {
        // Keyboard Input
        m_enterAction = new InputAction(binding: "<Keyboard>/enter");
        m_enterAction.performed += _ => { if (m_okButton.interactable) LevelManager.GetInstance().LoadLevel(m_selectedId); };

        // Interface & Behavior Setup
        m_okButton.onClick.AddListener(() => LevelManager.GetInstance().LoadLevel(m_selectedId));
        m_okButton.interactable = false;
    }
    private void InitializeVariable() {
        m_selectedId = -1;
        if (m_selectedIdText != null)
            m_selectedIdText.text = "OK";
    }

    void OnEnable()
    {
        m_upArrowAction.Enable();
        m_downArrowAction.Enable();
        if (m_okButton != null)
            m_enterAction.Enable();
    }
    void OnDisable()
    {
        m_upArrowAction.Disable();
        m_downArrowAction.Disable();
        if (m_okButton != null)
            m_enterAction.Disable();
    }
        
    public void SelectLevel(int level) {
        Debug.Log("Selecting level " + level);
        if (m_selectedId >= 0) m_buttons[m_selectedId].OnPointerExit(null);            
        m_selectedId = level;
        m_buttons[m_selectedId].Select();
        LevelManager.GetInstance().SetLevel(m_selectedId);
        m_selectedIdText.text = "" + (m_selectedId + 1);
        m_okButton.interactable = true;
    }
    public void SetupButtonAppearance(int atButtonIdx, string text, Sprite sprite) {
        // TODO: m_buttons[atButtonIdx].onClick.AddListener(...); Sound or Particles
        m_images[atButtonIdx].sprite = sprite;
        m_TMPs[atButtonIdx].text = text;
    }
}

