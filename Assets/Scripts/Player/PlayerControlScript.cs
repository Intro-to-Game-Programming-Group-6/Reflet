using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlScript : MonoBehaviour
{
    private static PlayerControlScript instance;

    #region Player Components
    [Header("Player Components")]
    [HideInInspector][SerializeField] private Rigidbody2D rb;
    [HideInInspector][SerializeField] private SpriteRenderer playerSprite;
    [HideInInspector][SerializeField] private Animator animator;
    #endregion
    
    #region Movement Variables
    [Header("Movement Variables")]
    [SerializeField] public bool canMove;
    [SerializeField] public float movementspeed = 3f;
    [HideInInspector][SerializeField] private Vector2 movementInput;
    [HideInInspector][SerializeField] private Vector2 dashDirection;
    #endregion
    
    #region Dash Variables
    [Header("Dash Variables")]
    [SerializeField] DashManager dashManager;
    [SerializeField] public float dashSpeed = 3f;
    [SerializeField] public int dashID = 2;
    [SerializeField] public float dashDuration = 0.5f;
    [SerializeField] public float dashCooldown = 1.5f;
    [SerializeField] public int dashCounter;
    [SerializeField] public TrailRenderer trail;
    [HideInInspector][SerializeField] public bool canDash = true;
    [HideInInspector][SerializeField] public bool currentlyDashing;
    #endregion

    #region Reflect Variables
    [Header("Reflect Variables")]
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public GameObject reflector;
    [SerializeField] bool mirrorRotate;
    [HideInInspector][SerializeField] GameObject shield;
    [HideInInspector][SerializeField] bool isReflecting = false;
    [HideInInspector][SerializeField] float orbitRadius = 3f;
    [HideInInspector][SerializeField] int numShields = 4;
    [HideInInspector][SerializeField] float rotationSpeed = 100f;
    [HideInInspector][SerializeField] public float attackRange = 0f;
    [HideInInspector][SerializeField] List<GameObject> tameng = new List<GameObject>();
    #endregion

    private float shield_time;
    private float max_shield_time = 3f;
    private float shield_cooldown;
    private float max_shield_cooldown = 2f;
    //

    //all about bullet stealing
    GameObject bullet_holder;

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
        dashManager = GetComponent<DashManager>();
        trail = GetComponent<TrailRenderer>();

        rb.gravityScale = 0f;
        trail.emitting = true;
        isReflecting = false;
        shield_time = max_shield_time;
        shield_cooldown = 0f;
    }

    void Update()
    {
        ManageSprite();
        ManageMovement();
        ManageShieldAction();
    }

    void ManageMovement()
    {
        if (currentlyDashing)
        {
            trail.emitting = true;
            return;
        }
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

    void ManageSprite()
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
    }

    void ManageShieldAction()
    {
        if (isReflecting)
        {
            Reflect();
            RotateShields();
            shield_time -= Time.deltaTime;
            if (shield_time <= 0f)
            {
                isReflecting = false;
                shield_time = max_shield_time;
                Destroy(shield);
                DestroyRotating();
                shield_cooldown = max_shield_cooldown;
                return;
            }
        }
        shield_cooldown -= Time.deltaTime;
    }

    void Reflect()
    {
        Vector2 clickdirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        clickdirection = clickdirection.normalized;

        float angle = Mathf.Atan2(clickdirection.y, clickdirection.x);
        angle = (angle + 2 * Mathf.PI) % (2 * Mathf.PI);
        angle = angle * Mathf.Rad2Deg;

        Vector2 spawnposition = (Vector2)spawnPoint.position + clickdirection * attackRange;
    
        if (shield == null)
        {
            shield = Instantiate(reflector, spawnposition, Quaternion.Euler(0, 0, angle - 90), gameObject.transform);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shield.GetComponent<Collider2D>(), true);
        }
        else
        {
            shield.transform.position = spawnposition;
            shield.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
    void CreateOrbitingShields()
    {
        if (isReflecting) return;
        for (int i = 0; i < numShields; i++)
        {
            // Debug.Log($"Shield no: {i}");
            float angle = i * (360f / numShields);
            Vector2 orbitPosition = rb.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitRadius;
            GameObject generated = Instantiate(reflector, orbitPosition, Quaternion.identity, gameObject.transform);
            tameng.Add(generated);
        }
        // Debug.Log($"Number of shields created: {tameng.Count}");
    }
    void RotateShields()
    {
        foreach (GameObject singular_shield in tameng)
        {
            GameObject kaca = singular_shield;
            if (kaca != null)
            {
                Vector2 shieldPosition = (Vector2)kaca.transform.position - (Vector2)rb.transform.position;
                float angle = Mathf.Atan2(shieldPosition.y, shieldPosition.x) * Mathf.Rad2Deg;

                angle += rotationSpeed * Time.deltaTime;
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
        if (context.performed && shield_cooldown <= 0f)// && Sword.GetInstance() == null)
        {
            if(mirrorRotate)
            {
                CreateOrbitingShields();
            }
            isReflecting = true;
        }
        else if (context.canceled)
        {
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
            dashManager.StartDash(this);
        }
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if(context.performed && PlayerManager.GetInstance().canHeal)
        {
            PlayerManager.GetInstance().Heal();
        }
    }
}
