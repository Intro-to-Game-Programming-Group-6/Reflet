using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Health")]
    [SerializeField] private int m_maxHealthPoint;
    [SerializeField] private int m_healthPoint;
    public int maxHealth { get { return m_maxHealthPoint; } }
    public int currentHealth { get { return m_healthPoint; } }

    [Header("Stamina")]
    [SerializeField] private int m_maxStaminaPoint;
    [SerializeField] private int m_StaminaPoint;
    public int maxStamina { get { return m_maxStaminaPoint; } }
    public int currentStamina { get { return m_StaminaPoint; } }
    
    [Header("Vial")]
    [SerializeField] private int m_maxVialPoint;
    [SerializeField] private int m_useVialPoint;
    [SerializeField] private int m_vialPoint;
    [SerializeField] public bool m_canHeal;
    public int maxVial { get { return m_maxVialPoint; } }
    public int useVial { get { return m_useVialPoint; } }
    public int currentVial { get { return m_vialPoint; } }
    public bool CanHeal { get { return m_canHeal; } set { m_canHeal = value; } }

    public bool multiply;
    public int bulletMultiplier;
    
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

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        m_healthPoint = m_maxHealthPoint;
        HealthController.GetInstance().SetMax(m_maxHealthPoint);
        HealthController.GetInstance().SetValue(m_healthPoint);

        m_StaminaPoint = m_maxStaminaPoint;
        StaminaController.GetInstance().SetMax(m_maxStaminaPoint);
        StaminaController.GetInstance().SetValue(m_StaminaPoint);

        m_vialPoint = 0;
        VialController.GetInstance().SetMax(m_maxVialPoint);
        VialController.GetInstance().SetUse(m_useVialPoint);
        VialController.GetInstance().SetValue(m_vialPoint);

        m_canHeal = false;
    }

    public void AdjustHealth(int deltaHealth) {
        m_healthPoint += deltaHealth;

        if (m_healthPoint > m_maxHealthPoint)
        {
            m_healthPoint = m_maxHealthPoint;
        }
        if (m_healthPoint < 0)
        {
            m_healthPoint = 0;
        }
        HealthController.GetInstance().SetValue(m_healthPoint);
    }

    public void AddVialPoint(int deltaPoint)
    {
        m_vialPoint += deltaPoint;

        if(m_vialPoint >= m_useVialPoint)
        {
            m_canHeal = true;
        }

        if(m_vialPoint >= m_maxVialPoint)
        {
            
            m_vialPoint = m_maxVialPoint;
        }

        VialController.GetInstance().SetValue(m_vialPoint);
    }

    public bool Heal()
    {
        if(m_vialPoint < m_useVialPoint)
        {
            return false;
        }
        m_canHeal = false;
        AdjustHealth(1);
        m_vialPoint -= m_useVialPoint;
        VialController.GetInstance().SetValue(m_vialPoint);
        return true;
    }

    public void AdjustStaminaPoint(int deltaPoint)
    {
        m_StaminaPoint += deltaPoint;

        if(m_StaminaPoint >= m_maxStaminaPoint)
        {
            m_StaminaPoint = m_maxStaminaPoint;
        }
        else if(m_StaminaPoint <= 0)
        {
            m_StaminaPoint = 0;
        }        

        StaminaController.GetInstance().AddValue(deltaPoint);
    }
}
