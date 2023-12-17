using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Health")]
    [SerializeField] private float m_maxHealthPoint;
    [SerializeField] private float m_healthPoint;
    public float maxHealth { get { return m_maxHealthPoint; } }
    public float currentHealth { get { return m_healthPoint; } }

    [Header("Stamina")]
    [SerializeField] private float m_maxStaminaPoint;
    [SerializeField] private float m_currentStamina;
    public float maxStamina { get { return m_maxStaminaPoint; } }
    public float currentStamina { get { return m_currentStamina; } }
    
    [Header("Vial")]
    [SerializeField] private float m_maxVialPoint;
    [SerializeField] private float m_useVialPoint;
    [SerializeField] private float m_vialPoint;
    [SerializeField] public bool m_canHeal;
    public float maxVial { get { return m_maxVialPoint; } set { m_maxVialPoint = value; } }
    public float useVial { get { return m_useVialPoint; } set { m_maxVialPoint = value; } }
    public float currentVial { get { return m_vialPoint; } }
    public bool CanHeal { get { return m_canHeal; } set { m_canHeal = value; } }

    public bool multiply;
    public int bulletMultiplier;
    public int bulletCaptureProgress, bulletCaptureLimit;
    public GameObject stolen_bullet_holder;
    
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
        m_healthPoint = 4f;// m_maxHealthPoint;
        HealthController.GetInstance().SetMax(m_maxHealthPoint);
        HealthController.GetInstance().SetValue(m_healthPoint);

        m_currentStamina = m_maxStaminaPoint;
        StaminaController.GetInstance().SetMax(m_maxStaminaPoint);
        StaminaController.GetInstance().SetValue(m_currentStamina);

        m_vialPoint = 0;
        VialController.GetInstance().SetMax(m_maxVialPoint);
        VialController.GetInstance().SetUse(m_useVialPoint);
        VialController.GetInstance().SetValue(m_vialPoint);

        m_canHeal = false;

        m_currentStamina = m_maxStaminaPoint;

        bulletCaptureProgress = 0;
        bulletCaptureLimit = 3;
        //m_staminaController.SetMax(m_maxStamina);
        //m_staminaController.SetValue(m_currentStamina);
    }
    public void ModifyStaminaCapacity(float val)
    {
        m_maxStaminaPoint = val;
        StaminaController.GetInstance().SetMax(val);
    }
    public void RoundHP()
    {
        m_healthPoint = Mathf.Round(m_healthPoint);
        HealthController.GetInstance().SetValue(m_healthPoint);
    }

    public bool CanUseShield()
    {
        return (m_currentStamina > 0f);
    }

    public float GetStamina()
    {
        return m_currentStamina;
    }

    public bool WillSteal()
    {
        return (bulletCaptureProgress >= bulletCaptureLimit)  && (stolen_bullet_holder == null);
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

    public bool Heal(float HealValue)
    {
        if(m_vialPoint < m_useVialPoint)
        {
            return false;
        }
        m_canHeal = false;
        AdjustHealth(10);
        m_vialPoint -= m_useVialPoint;
        VialController.GetInstance().SetValue(m_vialPoint);
        return true;
    }

    public bool EmptyVial()
    {
        m_canHeal = false;
        m_vialPoint = 0;
        VialController.GetInstance().SetValue(m_vialPoint);
        return true;
    }

    public void AdjustStaminaPoint(float deltaPoint)
    {
        m_currentStamina += deltaPoint;

        if(m_currentStamina >= m_maxStaminaPoint)
        {
            m_currentStamina = m_maxStaminaPoint;
        }
        else if(m_currentStamina <= 0)
        {
            m_currentStamina = 0;
        }        

        StaminaController.GetInstance().AddValue(deltaPoint);
    }

    public void StealProjectile(GameObject bullet)
    {
        stolen_bullet_holder = Instantiate(bullet, transform.position, Quaternion.identity);
        stolen_bullet_holder.SetActive(false);

        BaseBulletBehavior bulletbehav = stolen_bullet_holder.GetComponent<BaseBulletBehavior>();
        bulletbehav.PlayerForceOwnership();

        Destroy(bullet);
    }
}
