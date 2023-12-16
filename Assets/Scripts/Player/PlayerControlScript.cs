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
    [HideInInspector] [SerializeField] private List<float> angle_pos = new List<float>();
    [HideInInspector] [SerializeField] private List<GameObject> rotateshields = new List<GameObject>();

    #endregion

    #region Heal/Vial Action Variables
    [Header("Vial Variables")]
    public HealActionManager healManager;
    public int HealID =2;
    public float aoeHealRadius = 3;
    public float aoeHealTime = 100f;
    public float aoeHealTotal = 3f;
    #endregion

    private bool isSprinting;
    public float sprintspeed = 4.25f;

    GameObject bullet_holder;

    private float reflectionTimer = 0f;
    private float reflectionInterval = 0.1f; // 1 second interval

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
        // shield_time = max_shield_time;
        // shield_cooldown = 0f;
        mirrorRotate = true;
        numShields = 4;

        aoeHealRadius = 3;
        aoeHealTime = 10f;
        aoeHealTotal = 10f;

        //CreateOrbitingShields();
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

        trail.emitting = false;
    }

    void ManageShieldAction()
    {
        //Debug.Log("current stamina is: " + PlayerManager.GetInstance().GetStamina());
        RotateShields();

        reflectionTimer -= Time.deltaTime;

        if (reflectionTimer <= 0f)
        {
            reflectionTimer = reflectionInterval;
            if (isReflecting)
                PlayerManager.GetInstance().AdjustStaminaPoint(-1);
            else
                PlayerManager.GetInstance().AdjustStaminaPoint(1);
        }

        if (isReflecting)
        {
            Reflect();
            if (PlayerManager.GetInstance().currentStamina <= 0f)
            {
                isReflecting = false;
                Destroy(shield);
            }
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
        if (rotateshields.Count == numShields) return;

        if (isReflecting) return;

        for (int i = 0; i < numShields; i++)
        {
            float angle = i * (360f / numShields);
            Vector2 orbitPosition = rb.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitRadius;
            GameObject generated = Instantiate(reflector, orbitPosition, Quaternion.identity, gameObject.transform);
            generated.tag = "RotatingReflect";
            angle_pos.Add(angle);
            rotateshields.Add(generated);
        }
    }
    void RotateShields()
    {
        foreach (GameObject singular_shield in rotateshields)
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

    /*
    void DestroyRotating()
    {
        foreach (GameObject singular_shield in tameng)
        {
            if (singular_shield != null)
                Destroy(singular_shield);
        }
        tameng.Clear();
    }
    */
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
        if (context.performed && PlayerManager.GetInstance().currentStamina > 0)// && Sword.GetInstance() == null)
        {
            PlayerManager.GetInstance().AdjustStaminaPoint(-5);
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
        if(context.performed)// && PlayerManager.GetInstance().m_canHeal)
        {
            healManager.StartHeal(this);
        }
    }

    /*
    public void FindBullet()
    {
        float steal_distance = 5f;
        float thickness = 5f;
        Vector2 boxSize = new Vector2(thickness, steal_distance);

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);//, Vector2.SignedAngle(Vector2.right, clickdirection));
        //Debug.DrawRay(rb.transform.position, clickdirection.normalized * steal_distance, Color.red, 1f);
        foreach (Collider2D collider in hitColliders)
        {
            float distanceToPlayer = Vector2.Distance(collider.transform.position, rb.transform.position);
            if (distanceToPlayer > steal_distance) continue;

            // Check the tag of the object
            if (collider.CompareTag("Bullet"))
            {
                // Call a method to steal the projectile
                StealProjectile(collider.gameObject);
                break; // Break the loop after stealing the first projectile (adjust if needed)
            }
        }
    }

    public void StealProjectile(GameObject bullet)
    {
        bullet_holder = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet_holder.SetActive(false);

        BaseBulletBehavior bulletbehav = bullet_holder.GetComponent<BaseBulletBehavior>();
        bulletbehav.PlayerForceOwnership();

        Destroy(bullet);
    }
    */
    public void ShootStolenProjectile()
    {
        if (PlayerManager.GetInstance().stolen_bullet_holder != null)
        {
            Debug.Log("peluru lagi dibuang");
            float bulletSpeed = 15f;
            BaseBulletBehavior bulletbehav = PlayerManager.GetInstance().stolen_bullet_holder.GetComponent<BaseBulletBehavior>();
            string get_bullet_type = bulletbehav.GetBulletType();
            PlayerManager.GetInstance().stolen_bullet_holder.SetActive(true);
            PlayerManager.GetInstance().stolen_bullet_holder.transform.position = transform.position;
            // Make the projectile reappear in the game view
            GameObject newProjectile = PlayerManager.GetInstance().stolen_bullet_holder;            
            ActivateBullet(newProjectile, get_bullet_type);

            Vector2 direction = (CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            newProjectile.GetComponent<Transform>().right = direction; //work now but don't know why
            PlayerManager.GetInstance().stolen_bullet_holder = null;
        }
    }

    public void ActivateBullet(GameObject newProjectile, string bullet_type)
    {
        switch (bullet_type)
        {
            case "ExplosiveBullet":
                ExplosiveBullet explode = newProjectile.GetComponent<ExplosiveBullet>();
                CircleCollider2D explode_collid = newProjectile.GetComponent<CircleCollider2D>();
                explode.PlayerForceOwnership();
                explode.enabled = true;
                explode_collid.enabled = true;
                break;

            case "BouncingBullet":
                BouncingBullet bounce = newProjectile.GetComponent<BouncingBullet>();
                CircleCollider2D collid = newProjectile.GetComponent<CircleCollider2D>();
                bounce.PlayerForceOwnership();
                bounce.enabled = true;
                collid.enabled = true;
                break;

            case "NormalBullet":
                NormalBullet normal = newProjectile.GetComponent<NormalBullet>();
                CircleCollider2D normal_collid = newProjectile.GetComponent<CircleCollider2D>();
                normal.PlayerForceOwnership();
                normal.enabled = true;
                normal_collid.enabled = true;
                break;

            case "PiercingBullet":
                PiercingBullet pierce = newProjectile.GetComponent<PiercingBullet>();
                PolygonCollider2D pierce_collid = newProjectile.GetComponent<PolygonCollider2D>();
                pierce.PlayerForceOwnership();
                pierce.enabled = true;
                pierce_collid.enabled = true;
                break;
        }
        return;
    }

    public void OnSteal(InputAction.CallbackContext context)
    {
        if (context.performed && PlayerManager.GetInstance().stolen_bullet_holder != null)
        {
            ShootStolenProjectile();
            if (bullet_holder == null)
                Debug.Log("bullet shot");
        }
    }
}
