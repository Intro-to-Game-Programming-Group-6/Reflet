using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float maxHealth { get { return m_maxHealthPoint; } }
    public float currentHealth { get { return m_healthPoint; } }

    [SerializeField] private float m_maxHealthPoint;
    [HideInInspector][SerializeField] private float m_healthPoint;

    public int maxVial { get { return m_maxVialPoint; } }
    public int currentVial { get { return m_vialPoint; } }

    [SerializeField] private int m_maxVialPoint;
    [HideInInspector][SerializeField] private int m_vialPoint;


    [SerializeField] private HealthController m_heartController;
    [SerializeField] private BarController m_vialController;

    [SerializeField] public bool m_canHeal;
    public bool CanHeal { get { return m_canHeal; } set { m_canHeal = value; } }


    [Header("Stamina")]
    [SerializeField] private float m_maxStamina;
    [HideInInspector] [SerializeField] private float m_currentStamina;
    [SerializeField] private HealthController m_staminaController;

    public float maxStamina { get { return m_maxStamina; } }
    public float currentStamina { get { return m_currentStamina; } }


    public static PlayerManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_healthPoint = 1;// m_maxHealthPoint;
        m_heartController.SetMax(m_maxHealthPoint);
        m_heartController.SetValue(m_healthPoint);

        m_vialPoint = 0;
        m_vialController.SetMax(m_maxVialPoint);
        m_vialController.SetValue(m_vialPoint);

        m_canHeal = false;

        m_currentStamina = m_maxStamina;
        //m_staminaController.SetMax(m_maxStamina);
        //m_staminaController.SetValue(m_currentStamina);
    }

    public void AdjustHearts(float deltaHeart) {
        m_maxHealthPoint += deltaHeart;
        AdjustHealth(0);
    }

    public void ModifyStamina(float val)
    {
        if (m_currentStamina >= m_maxStamina  && val > 0f) return;
        
        m_currentStamina += val;
        if (m_currentStamina < 0f)
            m_currentStamina = 0f;
    }

    public void ShieldActivationCost(float percentage)
    {
        ModifyStamina(maxStamina * percentage);
    }

    public bool CanUseShield()
    {
        return (m_currentStamina > 0f);
    }

    public float GetStamina()
    {
        return m_currentStamina;
    }

    public void AdjustHealth(float deltaHealth) {
        m_healthPoint += deltaHealth;

        if (m_healthPoint > m_maxHealthPoint)
        {
            m_healthPoint = m_maxHealthPoint;
        }
        if (m_healthPoint < 0)
        {
            m_healthPoint = 0;
        }
        m_heartController.SetValue(m_healthPoint);
    }

    public void AddVialPoint(int deltaPoint)
    {
        m_vialPoint += deltaPoint;

        if(m_vialPoint >= m_maxVialPoint)
        {
            m_canHeal = true;
            m_canHeal = true;
            m_vialPoint = m_maxVialPoint;
        }

        m_vialController.SetValue(m_vialPoint);
    }

    public bool Heal(float HealValue)
    {
        if(m_vialPoint < m_maxVialPoint)
        {
            // TODO: cannot anim
            return false;
        }
        m_canHeal = false;
        AdjustHealth(HealValue);
        m_vialPoint = 0;
        m_vialController.SetValue(m_vialPoint);
        return true;
    }

    public bool EmptyVial()
    {
        if (m_vialPoint < m_maxVialPoint)
        {
            // TODO: cannot anim
            return false;
        }
        m_canHeal = false;
        m_vialPoint = 0;
        m_vialController.SetValue(m_vialPoint);
        return true;
    }
}
