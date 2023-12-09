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
    [HideInInspector]
    public DashManager Dash_Manager;
    public int DashID;
    public float DashSpeed;
    public float DashDuration;
    public float DashCooldown;
    public int DashCounter;
    public TrailRenderer trail;
    //

    //all about reflecting
    bool isReflecting;
    //Rotating Shields.
    private float orbitRadius = 3f;
    private int numShields = 4;
    private float rotationSpeed = 100f;
    private List<GameObject> tameng = new List<GameObject>();
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
        Dash_Manager = GetComponent<DashManager>();
        trail = GetComponent<TrailRenderer>();
        rb.gravityScale = 0f;
        canDash = true;
        DashDuration = 0.5f;
        DashCooldown = 1.5f;
        DashID = 2;
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
        if(bullet_holder != null)
        Debug.Log(bullet_holder);
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
        if (isReflecting) return;
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
        if (context.performed && shield_cooldown<=0f)// && Sword.GetInstance() == null)
        {
            //Reflect();
            CreateOrbitingShields();
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

    public void FindBullet()
    {
        float steal_distance = 4f;
        float thickness = 2f;
        Vector2 clickdirection = CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        clickdirection = clickdirection.normalized * steal_distance;
        Vector2 boxSize = new Vector2(thickness, steal_distance);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);

        foreach (Collider2D collider in hitColliders)
        {
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
        Destroy(bullet);
        Debug.Log("projectile stolen!");
    }

    public void ShootStolenProjectile()
    {
        Debug.Log(bullet_holder);
        if (bullet_holder != null)
        {
            Debug.Log("peluru lagi dibuang");
            float bulletSpeed = 15f;
            bullet_holder.SetActive(true);
            bullet_holder.transform.position = transform.position;
            // Make the projectile reappear in the game view
            GameObject newProjectile = bullet_holder;// Instantiate(bullet_holder, transform.position, Quaternion.identity);
            BaseBulletBehavior bulletbehav = newProjectile.GetComponent<BaseBulletBehavior>();
            string get_bullet_type = bulletbehav.GetBulletType();
            bulletbehav.PlayerForceOwnership();

            BouncingBullet bounce = newProjectile.GetComponent<BouncingBullet>();
            CircleCollider2D collid = newProjectile.GetComponent<CircleCollider2D>();
            bounce.enabled = true;
            collid.enabled = true;
            //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collid, true);
            // Print information about each component

            Vector2 direction = (CameraInstance.GetInstance().GetCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            newProjectile.GetComponent<Transform>().right = direction; //work now but don't know why
            bullet_holder = null;
        }
    }

    public void OnSteal(InputAction.CallbackContext context)
    {
        if (context.performed && bullet_holder == null)
        {
            FindBullet();
            //return;
        }
        else if (context.performed && bullet_holder != null)
        {
            ShootStolenProjectile();
            if (bullet_holder == null)
                Debug.Log("bullet shot");
        }
    }

    public void tembakasal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootStolenProjectile();
            Debug.Log("nembak");
        }
    }
}
