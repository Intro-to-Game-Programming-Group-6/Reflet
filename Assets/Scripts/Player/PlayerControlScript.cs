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

    void Start()
    {
        //mainBody = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();//.gravity = 0f;
        playersprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0f;
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
}
