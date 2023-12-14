using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlScript : MonoBehaviour
{
    private static PlayerControlScript instance;

    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private Animator animator;
    // private bool isSprinting;

    private Vector2 movementInput;
    private Vector2 dashDirection;
    
    public float movementspeed = 3f;
    public float attackRange = 0f;
    public float dashSpeed = 3f;
    // public float sprintspeed = 4.25f;
    public bool currentlyDashing, canDash;

    public Transform spawnPoint;
    public GameObject reflector;
    public Dash DashAbility;

    GameObject shield;

    //all about dash.
    [HideInInspector]
    public DashManager Dash_Manager;
    public int DashID;
    public float DashSpeed;
    public float DashDuration;
    public float DashCooldown;
    public int DashCounter;
    public TrailRenderer trail;
    public bool mirrorRotate;
    //

    //all about reflecting
    bool isReflecting;
    //Rotating Shields.
    private float orbitRadius = 3f;
    private int numShields = 4;
    private float rotationSpeed = 100f;
    private List<GameObject> tameng = new List<GameObject>();
    //

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static PlayerControlScript GetInstance()
    {
        return instance;
    }

    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Dash_Manager = GetComponent<DashManager>();
        trail = GetComponent<TrailRenderer>();
        rb.gravityScale = 0f;
        canDash = true;
        DashDuration = 0.5f;
        DashCooldown = 1.5f;
        DashID = 2;
        trail.emitting = true;
        isReflecting = false;
    }

    void Update()
    {
        if (rb.velocity == Vector2.zero)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        if (movementInput.x > 0f)
        {
            playerSprite.flipX = false;
        }
        else if (movementInput.x < 0f)
        {
            playerSprite.flipX = true;
        }

        if (currentlyDashing)
        {
            trail.emitting = true;
            return;
        }

        //if (DashAbility.Ability_Status == AbilityStatus.ACTIVE) return;
        // if (isSprinting)
        // {
        //     rb.velocity = (movementInput) * sprintspeed;
        // }
        // else
        // {
        //     rb.velocity = (movementInput) * movementspeed;
        // }

        rb.velocity = (movementInput) * movementspeed;

        trail.emitting = false;
        if (isReflecting)
        {
            Reflect();
            RotateShields();
        }
    }

    void Reflect()
    {
        Vector2 clickdirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        clickdirection = clickdirection.normalized;

        float angle = Mathf.Atan2(clickdirection.y, clickdirection.x);
        angle = (angle + 2 * Mathf.PI) % (2 * Mathf.PI);
        angle = angle * Mathf.Rad2Deg;

        Vector2 spawnposition = (Vector2)spawnPoint.position + clickdirection * attackRange;
        //Debug.Log(spawnposition);
        //GameObject instantRef = Instantiate(reflector, spawnposition, Quaternion.Euler(0, 0, angle - 90), gameObject.transform);
        if (shield == null)
        {
            shield = Instantiate(reflector, spawnposition, Quaternion.Euler(0, 0, angle - 90), gameObject.transform);
        }
        else
        {
            shield.transform.position = spawnposition;
            shield.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

    }

    void CreateOrbitingShields()
    {
        if (tameng.Count == numShields) return;
        for (int i = 0; i < numShields; i++)
        {
            Debug.Log($"Shield no: {i}");
            float angle = i * (360f / numShields);
            Vector2 orbitPosition = rb.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitRadius;
            GameObject generated = Instantiate(reflector, orbitPosition, Quaternion.identity, gameObject.transform);
            tameng.Add(generated);
        }
        Debug.Log($"Number of shields created: {tameng.Count}");
    }

    void RotateShields()
    {
        foreach (GameObject singular_shield in tameng)
        {
            GameObject kaca = singular_shield;
            //float angle = singular_shield.angle;
            if (kaca != null)
            {
                Vector2 shieldPosition = (Vector2)kaca.transform.position - (Vector2)rb.transform.position;
                float angle = Mathf.Atan2(shieldPosition.y, shieldPosition.x) * Mathf.Rad2Deg;

                angle += rotationSpeed * Time.deltaTime;
                //Debug.Log(angle);
                kaca.transform.position = (Vector2)rb.transform.position + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitRadius;
                kaca.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
        
    }

    void DestroyRotating()
    {
        foreach (GameObject singular_shield in tameng)
        {
            if (singular_shield != null)
                Destroy(singular_shield);
        }
        tameng.Clear();
    }

    // IEnumerator Dash()
    // {
    //     canDash = false;
    //     currentlyDashing = true;
    //     // Debug.Log("dashing " + ++count);
    //     dashDirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
    //     dashDirection = dashDirection.normalized;
    //     rb.velocity = dashDirection * dashSpeed;
    //     yield return new WaitForSeconds(0.5f); // Dash duration
    //     currentlyDashing = false;
    //     yield return new WaitForSeconds(1.5f); // Cool-down duration
    //     canDash = true;
    // }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
            movementInput.Normalize();
        }
        if (context.canceled)
        {
            movementInput = context.ReadValue<Vector2>();
            movementInput.Normalize();
        }

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed || Mouse.current.leftButton.isPressed)// && Sword.GetInstance() == null)
        {
            //Reflect();
            if(mirrorRotate)
            {
                CreateOrbitingShields();
            }
            isReflecting = true;
        }
        else if (context.canceled)
        {
            // Check if the shield is active and destroy it
            if (shield != null)
            {
                Destroy(shield);
                shield = null;
            }
            isReflecting = false;
            DestroyRotating();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            Dash_Manager.StartDash(this);
        }
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if(context.performed && PlayerManager.GetInstance().CanHeal)
        {
            PlayerManager.GetInstance().Heal();
        }
    }

    // public void OnSprint(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         isSprinting = true;
    //     }
    //     if (context.canceled)
    //     {
    //         isSprinting = false;
    //     }
    // }
}
