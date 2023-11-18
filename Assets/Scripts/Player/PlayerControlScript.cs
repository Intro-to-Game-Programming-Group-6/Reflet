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

    private Vector2 movementInput;

    public float angleOffset = 3f;
    public float movementspeed = 3f;
    public float attackRange = 0f;

    public bool interactable;

    public Transform swordSpawnPoint;
    public GameObject reflector;

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
        interactable = false;
    }

    void Update()
    {
        if (rb.velocity == Vector2.zero)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);
        if (movementInput.x >= 0f)
            playerSprite.flipX = false;
        else
            playerSprite.flipX = true;
        rb.velocity = (movementInput)*movementspeed;
        
    }

    void Reflect()
    {
        Vector2 clickdirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        clickdirection = clickdirection.normalized;

        float angle = Mathf.Atan2(clickdirection.y, clickdirection.x);
        angle = (angle + 2 * Mathf.PI) % (2 * Mathf.PI);
        angle = angle * Mathf.Rad2Deg;

        Vector2 spawnposition = (Vector2)swordSpawnPoint.position + clickdirection * attackRange;

        GameObject instantiatedObject = Instantiate(reflector, spawnposition, Quaternion.Euler(0, 0, angle - angleOffset));

        instantiatedObject.transform.parent = gameObject.transform;


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

    public bool GetInteractions()
    {
        return interactable;
    }

    public void ToggleInteractions(bool value)
    {
        interactable = value;
    }
}
