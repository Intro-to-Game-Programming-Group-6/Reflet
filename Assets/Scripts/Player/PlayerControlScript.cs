using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 movementInput;
    Rigidbody2D rb;
    SpriteRenderer playersprite;
    Animator animator;
    public float movementspeed = 3f;
    bool attacking;
    public Camera playercamera;
    public GameObject attackparticle;
    public GameObject debug;

    void Start()
    {
        //mainBody = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();//.gravity = 0f;
        playersprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0f;
        attacking = false;
    }

    void CheckIfAttack()
    {
        float attack_range = 0f;
        Vector2 clickdirection = playercamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - rb.transform.position;
        clickdirection = clickdirection.normalized;
        float angle = Mathf.Atan2(clickdirection.y, clickdirection.x);
        angle = (angle + 2 * Mathf.PI) % (2 * Mathf.PI);
        angle = angle * Mathf.Rad2Deg;
        Vector2 spawnposition = (Vector2)rb.transform.position + clickdirection * attack_range;
        GameObject attackParticleobj = Instantiate(attackparticle, spawnposition, Quaternion.Euler(angle+90f, -90f, -90f));
        Debug.Log(angle);
        //debug.transform.rotation = Quaternion.Euler(angle, -90f, -90f);
        ParticleSystem attackParticleSystem = attackParticleobj.GetComponent<ParticleSystem>();
        Destroy(attackParticleobj, attackParticleSystem.main.duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity == Vector2.zero)
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);
        if (movementInput.x >= 0f)
            playersprite.flipX = false;
        else
            playersprite.flipX = true;
        rb.velocity = (movementInput)*movementspeed;
        
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
        if (context.performed)
        {
            CheckIfAttack();
        }
    }
}
