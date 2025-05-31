using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [Header("Player Movements")]
    public float moveSpeed = 5f; // Move speed of the object
    public float jumpForce = 10f; // How much high the object can jump

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;
    private BoxCollider2D coll;
    private Vector2 moveInput;
    private float mobileInputX = 0f;
    private bool isCrouching = false;
    private Vector2 standingColliderSize;
    private Vector2 standingColliderOffset;
    private bool isAttacking = false;
    private bool isInvicible = false;

    // Knockback variables
    public float knockbackForce; // How much knockback effect will do to player
    public float knockbackCounter; // How long the effect of knockback will be
    public float knockbackTotalTime;  // Knockback time
    public bool knockbackFromRight; // Track where will knockback effect will affect to player

    // Animation set
    public enum MovementsState { idle, run, jump, fall, attack1, attack2, die, hit }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        playerController = new PlayerController();

        // Store original collider values
        standingColliderSize = coll.size;
        standingColliderOffset = coll.offset;
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Movements.Move.performed += OnMove;
        playerController.Movements.Move.canceled += OnMoveCancel;
        playerController.Movements.Jump.performed += OnJump;

        playerController.Attacks.BasicAttack.performed += Attack1;
    }

    private void OnDisable()
    {
        playerController.Disable();
        playerController.Movements.Move.performed -= OnMove;
        playerController.Movements.Move.canceled -= OnMoveCancel;
        playerController.Movements.Jump.performed -= OnJump;

        playerController.Attacks.BasicAttack.performed -= Attack1;
    }

    private void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    private void OnMoveCancel(InputAction.CallbackContext ctx) => moveInput = Vector2.zero;

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded() && !isCrouching) // Can't jump while crouching
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void Jump()
    {
        // Cek ulang grounded saat ini, dan jangan gunakan isJumping (karena bisa delay)
        if (isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }
    }

    private void Attack1(InputAction.CallbackContext ctx)
    {
        DoAttack1();
    }

    private void DoAttack1()
    {
        if (isGrounded() && !isJumping)
        {
            isAttacking = true;
            anim.SetInteger("state", (int)MovementsState.attack1);
            rb.linearVelocity = Vector2.zero;

            Invoke(nameof(EndAttack), 0.4f);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            moveInput = playerController.Movements.Move.ReadValue<Vector2>();
        }
        if (!isAttacking)
        {
            if (knockbackCounter <= 0)
            {
                Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed, rb.linearVelocity.y);
                rb.linearVelocity = targetVelocity;
            }
            else
            {
                if (knockbackFromRight == true)
                {
                    rb.linearVelocity = new Vector2(-knockbackForce, 0);
                }
                if (knockbackFromRight == false)
                {
                    rb.linearVelocity = new Vector2(knockbackForce, 0);
                }
                knockbackCounter -= Time.deltaTime;
            }

        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementsState state;

        float horizontal = moveInput.x != 0 ? moveInput.x : mobileInputX;

        if (horizontal > 0f)
        {
            state = MovementsState.run;
            sprite.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementsState.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementsState.idle;
        }

        if (rb.linearVelocity.y > 0.1f)
        {
            state = MovementsState.jump;
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            state = MovementsState.fall;
        }

        // Knockback / Hit animation
        if (knockbackCounter > 0)
        {
            state = MovementsState.hit;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        isJumping = false;
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    public void moveRight(bool isPressed)
    {
        if (isPressed)
            mobileInputX = 1f;
        else if (mobileInputX == 1f)
            mobileInputX = 0f;
    }

    public void moveLeft(bool isPressed)
    {
        if (isPressed)
            mobileInputX = -1f;
        else if (mobileInputX == -1f)
            mobileInputX = 0f;
    }

    public void mobileJump()
    {
        if (isGrounded())
            Jump();
    }

    public void mobileAttack()
    {
        DoAttack1();
    }
}