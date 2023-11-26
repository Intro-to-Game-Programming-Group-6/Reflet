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
    private bool currentlyDashing, canDash;

    public Transform spawnPoint;
    public GameObject reflector;
    

    int count = 0;

    void Awake()
    {
        if(instance == null)
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0f;
        canDash = true;
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
        if (currentlyDashing) return;
        if (isSprinting)
        {
            rb.velocity = (movementInput) * sprintspeed;
        }
        else
        {
            rb.velocity = (movementInput) * movementspeed;
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

        GameObject instantRef = Instantiate(reflector, spawnposition, Quaternion.Euler(0, 0, angle - 90), gameObject.transform);
    }

    IEnumerator Dash()
    {
        canDash = false;
        currentlyDashing = true;
        Debug.Log("dashing " + ++count);
        dashDirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        dashDirection = dashDirection.normalized;
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(0.5f);
        currentlyDashing = false;
        yield return new WaitForSeconds(1.5f);
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
        if (context.performed && Sword.GetInstance() == null)
        {
            Reflect();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(Dash());
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
