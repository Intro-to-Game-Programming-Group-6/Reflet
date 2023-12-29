using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Dash NormalDash;
    public DashBlink BlinkDash;

    [SerializeField] private GameObject skillIndicatorObject;
    private SkillIndicator skillIndicator;

    [SerializeField] public float dashSpeed = 5f;
    [SerializeField] private int dashID = 1;
    [SerializeField] public float dashDuration = 0.5f;
    [SerializeField] public float dashCastTime = 1.5f;
    [SerializeField] private float m_dashCooldown;

    public float dashCooldown
    {
        get { return m_dashCooldown; }
        set { m_dashCooldown = value; skillIndicator.MaximumCountdown = m_dashCooldown; }
    }
    [SerializeField] public int m_dashAvailability;
    public int dashAvailability
    {
        get { return m_dashAvailability; }
        set { m_dashAvailability = value; skillIndicator.SkillAvailability = m_dashAvailability; }
    }
    [SerializeField] private int m_dashMaxCharge;
    public int dashMaxCharge
    {
        get { return m_dashMaxCharge; }
        set { m_dashMaxCharge = value; }
    }

    [SerializeField] private float m_dashRefresh;
    public float dashRefresh
    {
        get { return m_dashRefresh; }
        set { m_dashRefresh = value; skillIndicator.ValueCountdown = m_dashRefresh; }
    }
    [SerializeField] public bool canDash;
    [SerializeField] private bool m_isDashing;
    public bool isDashing
    {
        get { return m_isDashing; }
        set { m_isDashing = value; animator.SetBool("isDashing", value); skillIndicator.IsPressed = value; }
    }

    [SerializeField] public ParticleSystem blinkParticle;
    [SerializeField] public float blinkRange = 5f;
    [HideInInspector] public UnityEvent onActivated;
    private Animator animator;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource normalPitchSource;
    [SerializeField] public AudioClip dashingAudioClip;
    [SerializeField] public AudioClip blinkingAudioClip;

    private void Start()
    {
        animator = GetComponent<Animator>();

        blinkParticle = GetComponent<ParticleSystem>();
        blinkParticle.Stop();

        skillIndicator = skillIndicatorObject.GetComponent<SkillIndicator>();
        skillIndicator.Key = "spc";
        if (skillIndicator != null){ Debug.LogError(gameObject.name + ": Missing Skill Indicator");}

        dashMaxCharge = 3;
        Debug.Log("dashMaxCharge:"+ dashMaxCharge);
        dashAvailability = 1;
        dashRefresh = 0;
        dashCooldown = 7f;
        dashCastTime = 1.5f;

        canDash = true;
    }

    void Update()
    {
        if (dashAvailability < dashMaxCharge)
        {
            bool cek = (dashRefresh >= dashCooldown);
            Debug.Log(""+ cek + "Add"+ dashCooldown);
            if (dashRefresh >= dashCooldown) {
                Debug.Log("Ceasd");

                if (dashAvailability == dashMaxCharge - 1) dashRefresh = 0f; 
                else dashRefresh -= (dashCooldown - Time.deltaTime);
                dashAvailability = dashAvailability + 1; 
            }
            else { dashRefresh += Time.deltaTime; }
        }
        if (dashAvailability > dashMaxCharge) Debug.LogError("Availability Exceeding Capacity");
    }

    public void StartDash(PlayerControlScript mainController)
    {
        switch (dashID)
        {
            case 1:
                if(dashAvailability > 0)
                    normalPitchSource.PlayOneShot(dashingAudioClip);
                StartCoroutine(NormalDash.GoDash(mainController, this));
                break;
            case 2:
                if (dashAvailability > 0)
                    normalPitchSource.PlayOneShot(blinkingAudioClip);
                StartCoroutine(BlinkDash.GoDash(mainController, this));
                break;
            case 3:
                break;
        }
    }

    void UpgradeDash(int upgrade_id, PlayerControlScript mainController, float bonus)
    {
        switch (upgrade_id)
        {
            case 1:
                dashCooldown += bonus;
                break;
            case 2:
                dashDuration += bonus;
                break;
            case 3:
                dashSpeed += bonus;
                break;
        }
    }
}
