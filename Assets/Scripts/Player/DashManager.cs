using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Dash NormalDash;
    public DashBlink BlinkDash;

    [SerializeField] public float dashSpeed = 5f;
    [SerializeField] public int dashID = 1;
    [SerializeField] public float dashDuration = 0.5f;
    [SerializeField] public float dashCastTime = 1.5f;
    [SerializeField] public float dashCooldown = 10f;
    [SerializeField] public int dashAvailability;
    [SerializeField] public int dashMaxCharge;
    [SerializeField] public float dashRefresh;
    [SerializeField] public bool canDash = true;
    [SerializeField] private bool m_isDashing;
    public bool isDashing
    {
        get { return m_isDashing; }
        set { m_isDashing = value; animator.SetBool("isDashing", value); }
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

        dashMaxCharge = 3;
        dashAvailability = 1;
        dashRefresh = 0;
        dashCastTime = 1.5f;

        canDash = true;
    }

    void Update()
    {
        if (dashAvailability < dashMaxCharge)
        {
            dashRefresh += Time.deltaTime; // Increment cooldown timer by Delta Time per frame;
            if (dashRefresh >= dashCooldown) // Exceed cooldown timer
            {
                dashRefresh -= dashCooldown;
                dashAvailability++;
            }
            if (dashAvailability == dashMaxCharge) dashRefresh = 0f; // Reset cooldown timer
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
        //shields_up.pitch = 1.4f;
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
