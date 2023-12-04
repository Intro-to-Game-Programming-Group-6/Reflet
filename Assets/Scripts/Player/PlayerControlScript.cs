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
    private bool isSprinting;

    private Vector2 movementInput;
    private Vector2 dashDirection;

    public float movementspeed = 3f;
    public float attackRange = 0f;
    public float dashSpeed = 3f;
    public float sprintspeed = 4.25f;
    public bool currentlyDashing, canDash;

    public Transform spawnPoint;
    public GameObject reflector;
    public Dash DashAbility;

    int count = 0;
    GameObject shield;

    //all about dash.
    public DashManager Dash_Manager;
    public int DashID;
    public float DashSpeed;
    public float DashDuration;
    public float DashCooldown;
    public int DashCounter;
    public TrailRenderer trail;
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
        trail.emitting = false;
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
        if (isSprinting)
        {
            rb.velocity = (movementInput) * sprintspeed;
        }
        else
        {
            rb.velocity = (movementInput) * movementspeed;
        }
        trail.emitting = false;
    }

    void Reflect()
    {
        Vector2 clickdirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        clickdirection = clickdirection.normalized;

        float angle = Mathf.Atan2(clickdirection.y, clickdirection.x);
        angle = (angle + 2 * Mathf.PI) % (2 * Mathf.PI);
        angle = angle * Mathf.Rad2Deg;

        Vector2 spawnposition = (Vector2)spawnPoint.position + clickdirection * attackRange;
        Debug.Log(spawnposition);
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

    IEnumerator Dash()
    {
        canDash = false;
        currentlyDashing = true;
        Debug.Log("dashing " + ++count);
        dashDirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        dashDirection = dashDirection.normalized;
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(0.5f); // Dash duration
        currentlyDashing = false;
        yield return new WaitForSeconds(1.5f); // Cool-down duration
        canDash = true;
    }

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
            Reflect();
        }
        else if (context.canceled)
        {
            // Check if the shield is active and destroy it
            if (shield != null)
            {
                Destroy(shield);
                shield = null;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            Dash_Manager.StartDash(this);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        if (context.canceled)
        {
            isSprinting = false;
        }
    }
}
