using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int maxHealth { get { return m_maxHealthPoint; } }
    public int currentHealth { get { return m_healthPoint; } }

    [SerializeField] private int m_maxHealthPoint;
    [SerializeField] private int m_healthPoint;

    public int maxVial { get { return m_maxVialPoint; } }
    public int currentVial { get { return m_vialPoint; } }

    [SerializeField] private int m_maxVialPoint;
    [SerializeField] private int m_vialPoint;


    [SerializeField] private BarController m_heartController;
    [SerializeField] private BarController m_vialController;

    [SerializeField] private bool m_canHeal;
    public bool CanHeal { get { return m_canHeal; } set { m_canHeal = value; } }
    

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
        m_heartController.SetMax(m_maxHealthPoint);
        m_heartController.SetValue(m_healthPoint);

        m_vialController.SetMax(m_maxVialPoint);
        m_vialController.SetValue(m_vialPoint);

        m_vialPoint = 0;
        m_canHeal = false;
    }

    public void AdjustHearts(int deltaHeart) {
        m_maxHealthPoint += deltaHeart;
        AddHealth(0);
    }

    public void AddHealth(int deltaHealth) {
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
            canHeal = true;
            m_vialPoint = m_maxVialPoint;
        }

        m_vialController.SetValue(m_vialPoint);
    }

    public bool VialFullState()
    {
        if(m_vialPoint < m_maxVialPoint)
        {
            // TODO: cannot anim
            return;
        }
        m_canHeal = false;
        AddHealth(1);
        m_vialPoint = 0;
        m_vialController.SetValue(m_vialPoint);
        // TODO: Vial.GetInstance().UpdateVial(vialPoint);
    }

    public bool VialFullState()
    {
        return m_vialPoint == m_maxVialPoint;
    }
}
